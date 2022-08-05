const hs = require("../HanSocket/HanSocket");
const skills = require("../Skills/skills");
const Vector2 = require("./Vector2");

class game
{
    constructor(players, id) {
        this.players = players;
        this.id = id;

        this.justDiedPlayer;

        this.loadedCount = 0;
        this.deadPlayers = 0;
        
        this.skillSelectCount  = 0; // 스킬 선택 카운트
        this.lastLostPlayerId  = -1; // 마지막으로 (세트)진 플레이어 아이디
        this.gameWonStackToWin = 3;
        this.setWonStackToWin  = 2;
        this.invincibleTimeMs  = 5000;
    }

    broadcast(payload, excludeIds = [-1,]) {
        this.players.forEach(ws => {

            // 제외
            if (excludeIds.findIndex(x => x == ws.id) != -1)
                return;
            
            hs.send(ws, payload);
        });
    }

    newLoop() {
        this.deadPlayers = 0;
        let targetId =
            (this.justDiedPlayer == null) ? -1 : this.justDiedPlayer.id;

        this.broadcast(hs.toJson(
            "newloop",
            JSON.stringify({
                skill: targetId,
                selectCount: this.skillSelectCount,
            }),
        ));


        this.justDiedPlayer   = null;
    }

    loaded(ws) {
        ++this.loadedCount;
        
        ws.setWon      = 0;
        ws.gameWon     = 0;
        ws.invincible  = false;
        ws.selectCount = 1;
        ws.hp          = 100;
        ws.skills = [];

        // 모든 클라이언트가 로딩 완료된 경우
        if (this.loadedCount >= this.players.length) {
            this.sendGamedata();

            this.skillSelectCount = 2;
            this.newLoop();
        }
        
    }

    sendGamedata() {
        const stat = new skills();
        
        let ids = [];
        this.players.forEach(ws => {
            ids.push(ws.id);
        });

        let gamedata = {
            // 플레이어 아이디 데이터
            players: ids,

            // 자신 아이디
            myId: -1,

            // 채력
            hp: stat.hp,
            damage: stat.damage,

            // 이동
            speed: stat.speed,
            jumpPower: stat.jumpPower,

            // 블럭 관련
            blockSize: stat.blocksize,
            blockRateFire: stat.ratefire,
            blockSpeed: stat.blockspeed,
            rotationSpeed: stat.rotationspeed,
        };

        this.players.forEach(ws => {
            gamedata.myId = ws.id;
            hs.send(ws, hs.toJson(
                "gamedata",
                JSON.stringify(gamedata)
            ));
        });
    }

    skillselected(ws, type, index) {
        // TODO: 스킬 적용
        if (--ws.selectCount < 0) return;

        ws.skills.push({ type, index });

        if (--this.skillSelectCount <= 0) {
            let pos = new Vector2(-4.0, 0.0);

            this.players.forEach(ws => {
                this.broadcast(hs.toJson(
                    "gamestart",
                    JSON.stringify({
                        id: ws.id,
                        pos: pos,
                    }),
                ));

                pos.x = 4;
                ws.selectCount = 0;
            });
        }
        
    }

    damage(damagedws) {
        let attackws = this.players.find(x => x != damagedws);
        let instance = null;
        console.log("Damaged");

        // attackws.skills
        //     .filter(x => x.type == 0)
        //     .forEach(skill => {
        //         instance =
        //             new skills(instance, attackws).skills[0][skills]();
        //     });
        
        console.log(instance);
    }
    
    dead(deadws) {
        ++this.deadPlayers;
        this.justDiedPlayer = deadws;

        if (this.deadPlayers >= this.players.length - 1) {
            let ws = this.players.find(x => x != deadws);
            ++ws.setWon;

            if (ws.setWon >= this.setWonStackToWin) { // 세트 승리
                ws.setWon     = 0;
                deadws.setWon = 0;
                ++ws.gameWon;
                
                if (ws.gameWon >= this.gameWonStackToWin) { // 게임 승리
                    this.gameEnd(ws.id);
                    return;
                }
                
                // 새 세트 실행
                this.skillSelectCount =
                    (this.lastLostPlayerId == deadws.id) ? 2 : 1;
                deadws.selectCount = this.skillSelectCount;

                this.lastLostPlayerId = deadws.id;
                this.newLoop();
                return;

            } else { // 리스폰 처리
                this.broadcast(hs.toJson(
                    "respawn",
                    JSON.stringify({
                        id: deadws.id,
                        pos: new Vector2(0.0, 0.0),
                    }),
                ));

                deadws.invincible = true;
                setTimeout(() => {
                    deadws.invincible = false;
                }, this.invincibleTimeMs);
            }
        }
    }
    
    gameEnd(winnerId, reason = "") {
        let payload = {
            winnerId: winnerId,
            reason: reason
        }

        this.players.forEach(ws => {
            hs.send(ws, hs.toJson(
                "gameend",
                JSON.stringify(payload)
            ));

            ws.game  = null;
            ws.match = false;
        });
    }

    left(leftws) {
        let idx = this.players.findIndex(x => x == leftws);
        this.players.splice(idx, 1);

        this.broadcast(hs.toJson(
            "left",
            JSON.stringify({
                id: leftws.id
            }),
        ));

        if (this.players.length <= 1) {
            let lastPlayerid = this.players.find(x => x != leftws);
            this.gameEnd(lastPlayerid.id, "Enemy left.");
        }

        leftws.game = null;
    }

}

module.exports = game;