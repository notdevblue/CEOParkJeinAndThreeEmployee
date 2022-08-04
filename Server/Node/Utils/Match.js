const hs    = require("../HanSocket/HanSocket.js");
const { setGameLoop, clearGameLoop } = require("./gameloop.js");
const write = require("./Logger.js");
const Vector2 = require("./Vector2.js");
const mysql = require("./mysql-extensions.js");

class Match
{
   constructor() {
      this.matches = [];
      this.matchid = 0;
   }

   currentQueueMatch() {
      let match = this.matches.find(x => x.queue);

      if (match == undefined) {
         match = this.newMatch();
      }

      return match;
   }

   startMatch() {
      this.matches.find(x => x.queue).start();
   }

   newMatch() {
      if (this.matches.findIndex(x => x.queue) != -1) return;

      write(`[II] New match created. ID: ${this.matchid + 1}`);

      this.matches[this.matchid] = new Game(this.matchid);
      return this.matches[this.matchid++];
   }
}

class Game
{
   constructor(id) {
      this.queue = true;
      this.players = [];
      this.bullets = [];
      this.id = id;

      this.moveloopId = -1;
      this.bulletloopId = -1;
      this.queueId = -1;
      this.timeoutId = -1;

      this.playerSpeed = 8;
      this.playerHp = 100;
      this.firedelay = 800; // ms

      this.shellAngle = 25.0;
      this.shellLifetime = 10000; // ms
      this.shellSpeed = 40.0;
      this.shellDamage = 10;
      this.shellCount = 4;
      this.shellId = 0;
      this.shellCollisionDistance = 0.5;

      this.halfAngle = this.shellAngle / 2.0;
      this.halfCount = this.shellCount / 2.0;

      this.queueTimeMs = 5000;
      this.loadingTimeoutMs = 120 * 1000; // 2분

      this.moveFps = 1000 / 30;
      this.bulletFps = 1000 / 15;

      this.mapMaxX = 25;
      this.mapMaxY = 25;
      this.mapMinX = -25;
      this.mapMinY = -25;

   }

   start() {
      this.queue = false;
      this.broadcast("gamestart", "");
   }

   broadcast(payload) {
      this.players.forEach(e => {
         hs.send(e, payload);
      });
   }

   join(ws) {
      let roomdataPayload;
      let roomdata = [];

      ws.position = new Vector2(0.0, 0.0);
      ws.hp = this.playerHp;
      ws.dead = false;
      ws.onfiredelay = false;
      ws.kill = 0;
      
      // 방 정보
      this.players.forEach(e => {
         roomdata.push({
            id: e.id,
            hp: e.hp,
            nickname: e.nickname,
            pos: e.position
         });
      });
      roomdataPayload = JSON.stringify({ userlist: roomdata });
      hs.send(ws, hs.toJson("roomdata", roomdataPayload));
      
      this.players[ws.id] = ws;

      const initpos = new Vector2(
         this.mapMinX + (Math.random() * this.mapMaxX * 2),
         this.mapMinY + (Math.random() * this.mapMaxY * 2)
      );

      ws.position = initpos;

      // 입장 이벤트
      const joinedPayload
         = hs.toJson("joined", JSON.stringify({
            id: ws.id,
            hp: ws.hp,
            nickname: ws.nickname,
            pos: initpos
         }));
      this.broadcast(joinedPayload);

      write(`[  ] Client ${ws.id} joined match: ${this.id}`, ws.ipAddr);

      if (this.moveloopId === -1) {
         this.processPosition();
      }
      if (this.bulletloopId === -1) {
         this.processBullet();
      } 

      // if (Object.keys(this.players).length >= 2) {
      //    this.queueId = setTimeout(() => {
      //       this.start();
      //    }, this.queueTimeMs);
      // }
   }

   leave(ws) {
      this.broadcast(hs.toJson("left", JSON.stringify({ id: ws.id })));

      delete this.players[ws.id];

      write(`[II] Client ${ws.id} left match: ${this.id}`, ws.ipAddr);

      // if (Object.keys(this.players).length < 2) {
      //    clearInterval(this.queueId);
      // }

      if (Object.keys(this.players).length <= 0) {
         write(`[II] Game ${this.id} loop terminated`);
         this.stopProcessingPosition();
         this.stopProcessingBullet();
      }
   }

   processPosition() {
      this.moveloopId = setGameLoop(deltaTime => {
         this.players.forEach(e => {
            if (e.dead) return;

            e.position.x +=
               e.deltaPosition.x * deltaTime * this.playerSpeed;
            e.position.y +=
               e.deltaPosition.y * deltaTime * this.playerSpeed;
            
            
            if (e.position.x < this.mapMinX)
               e.position.x = this.mapMinX + 0.1;
            if (e.position.y < this.mapMinY)
               e.position.y = this.mapMinY + 0.1;
            
            if (e.position.x > this.mapMaxX)
               e.position.x = this.mapMaxX - 0.1;
            if (e.position.y > this.mapMaxY)
               e.position.y = this.mapMaxY - 0.1;
            

            const payload = JSON.stringify({
               id: e.id,
               pos: e.position
            });

            this.players.forEach(ws => {
               ws.send(hs.toJson("moveto", payload));
            });
            
            e.deltaPosition.reset();
         });
      }, this.moveFps);
   }

   stopProcessingPosition() {
      clearGameLoop(this.moveloopId);
      this.moveloopId = -1;
   }

   move(ws, delta) {
      if (ws.dead) return;

      delta.x = Math.sign(delta.x);
      delta.y = Math.sign(delta.y);
      ws.deltaPosition.addself(delta);
   }

   processBullet() {
      this.bulletloopId = setGameLoop(deltaTime => {
         let bullets = [];
         let removeTargetIdx = [];

         this.bullets.forEach(e => {
            let keepgoing = true;

            // 라이프타임 처리
            if (e.firedat + e.lifetime <= Date.now()) {
               this.bullets.splice(this.bullets.indexOf(e), 1);
               return;
            }

            this.players.forEach(p => {
               if (p.id == e.firedby) return;

               let x = Math.abs(e.pos.x - p.position.x);
               let y = Math.abs(e.pos.y - p.position.y);
               
               // 충돌 처리
               if (Math.sqrt(x * x + y * y) <= this.shellCollisionDistance) {
                  keepgoing = false;
                  this.bullets.splice(this.bullets.indexOf(e), 1);
                  this.hit(p, e);
                  return;
               }
            });

            if (!keepgoing) return;

            e.pos.x += e.delta.x * deltaTime * e.speed;
            e.pos.y += e.delta.y * deltaTime * e.speed;

            bullets.push({
               id: e.id,
               pos: e.pos
            });
         });

         this.players.forEach(ws => {
            ws.send(hs.toJson("bulletdata", JSON.stringify({
               bullets: bullets
            })));
         })
      }, this.bulletFps);
   }

   stopProcessingBullet() {
      clearGameLoop(this.bulletloopId);
      this.bulletloopId = -1;
      this.bullets = [];
   }

   fire(ws, angle, firedPos) {
      if (ws.dead || ws.onfiredelay) return;

      ws.onfiredelay = true;
      setTimeout(() => {
         ws.onfiredelay = false;
      }, this.firedelay);

      for (let i = -this.halfCount; i <= this.halfCount; ++i) {
         // if (i == 0) continue;

         let shell = {
            id: this.shellId++,
            firedby: ws.id,
            firedat: Date.now(),
            lifetime: this.shellLifetime,
            pos: new Vector2(0, 0),
            delta: new Vector2(0, 0),
            speed: this.shellSpeed,
            angle: (angle + (Math.sign(i)
               * ((Math.random() * this.halfAngle)
                  + ((Math.abs(i) == this.halfCount) * this.halfAngle)))) * Math.PI / 180
         };

         shell.delta.x = Math.cos(shell.angle);
         shell.delta.y = Math.sin(shell.angle);
         shell.pos.x = firedPos.x;
         shell.pos.y = firedPos.y;

         this.bullets.push(shell);
      }
   }

   deletebullet(shell) {
      this.bullets.findIndex(x => x == shell);
   }

   hit(ws, shell) {
      if (ws.dead || !this.queue) return;

      ws.hp -= this.shellDamage;

      if (ws.hp <= 0) {
         this.dead(ws, shell);
         return;
      }

      const payload = JSON.stringify({
         id: ws.id,
         hp: ws.hp
      });

      this.broadcast(hs.toJson("hit", payload));
   }

   dead(ws, shell) {
      ws.dead = true;
      let killer = this.players[shell.firedby];

      const payload = JSON.stringify({
         id: ws.id,
         killedby: shell.firedby
      });
      
      this.broadcast(hs.toJson("dead", payload));

      try {
         mysql.updateKills(killer.nickname, 1);
         mysql.updateDeaths(ws.nickname, 1);
      } catch (error) {
         write(error);
      }

      if (!this.queue) {
         let notdeadCount = 0;
         this.players.forEach(e => {
            notdeadCount += e.dead;
         });

         if (notdeadCount <= 1) {
            this.broadcast(hs.toJson("win", ""));
            try {
               mysql.updateWon(killer.nickname, 1);
            } catch (error) {
               write(error);
            }
               
         }
      }
   }

   gamestart() {
      this.broadcast(hs.toJson("gamestart"));

      this.players.forEach(e => e.loaded = false);

      this.timeoutId = setTimeout(() => {
         this.players.forEach(e => {
            if (!e.loaded) {
               this.broadcast(hs.toJson("loadedtimeout",
                  JSON.stringify({
                     id: e.id
               })));
            }
         });
      }, this.loadingTimeoutMs);
   }

   loaded(ws) {
      ws.loaded = true;

      let allLoaded = true;

      this.players.forEach(e => {
         if (!e.loaded)
            allLoaded = false;
      });

      if (allLoaded) {
         try {
            clearInterval(this.timeoutId);
         } catch { }
            
         this.timeoutId = -1;

         this.broadcast(hs.toJson("allloaded", ""));
      }
   }
}

module.exports = new Match();