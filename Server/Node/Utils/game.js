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
        this.sendGamedata();
        this.deadPlayers = 0;
        let targetId =
            (this.justDiedPlayer == null) ? -1 : this.justDiedPlayer.id;
        
        const skill = new skills();

        let randomSkills = [];
        let sendList     = [];

        skill.skills.forEach(x => {
            x.forEach(y => {
                randomSkills.push(randomSkills.length);
            });
        });
        
        for (let i = 0; i < 5; ++i) {
            let idx = Math.floor(Math.random() * (randomSkills.length - 1));
            let skill = randomSkills.splice(idx, 1)[0];
            let type = -1;

            if (skill >= 13) {
                type = 5;
                skill -= 13;
            }
            else if (skill >= 7) {
                type = 4;
                skill -= 7;
            }
            else if (skill >= 6) {
                type = 3;
                skill -= 6;
            }
            else if (skill >= 4) {
                type = 2;
                skill -= 4;
            }
            else if (skill >= 3) {
                type = 1;
                skill -= 3;
            }
            else {
                type = 0;
            }
            
            sendList.push({
                type: type,
                skill: skill,
            });
        }

        this.broadcast(hs.toJson(
            "newloop",
            JSON.stringify({
                skill: targetId,
                selectCount: this.skillSelectCount,
                skillList: sendList
            })
        ));


        this.justDiedPlayer   = null;
    }

    loaded(ws) {

        ++this.loadedCount;
        ws.setWon        = 0;
        ws.gameWon       = 0;
        ws.invincible    = false;
        ws.selectCount   = 1;
        ws.skills        = [];
        ws.abliSkills    = [];
        
        this.resetPlayerValue(ws);
        
        // 모든 클라이언트가 로딩 완료된 경우
        if (this.loadedCount >= this.players.length) {
            this.skillSelectCount = 2;
            this.newLoop();
        }
    }

    resetPlayerValue(ws) {
        ws.damage        = 20;
        ws.hp            = 100;
        ws.maxhp         = 100;
        ws.blocksize     = 5;
        ws.blockspeed    = 5;
        ws.speed         = 4;
        ws.jumpPower     = 5;
        ws.ratefire      = 0.25;
        ws.rotationspeed = 7;
        ws.pushpower     = 10;
        ws.bomb          = false;
        ws.penetrate     = false;
        ws.hasShield     = false;
    }

    sendGamedata() {
        
        let ids = [];
        this.players.forEach(ws => {
            ids.push(ws.id);
        });

        
        this.players.forEach(ws => {
            this.applyStat(ws);
            let gamedata = {
                // 소켓 아이디
                id: ws.id,

                // 채력
                damage: ws.damage,
                hp: ws.hp,

                // 이동
                speed: ws.speed,
                jumpPower: ws.jumpPower,

                // 블럭 관련
                blockSize: ws.blocksize,
                blockRateFire: ws.ratefire,
                blockSpeed: ws.blockspeed,
                rotationSpeed: ws.rotationspeed,
                bomb: ws.bomb,
                penetrate: ws.penetrate,
                hasShield: ws.hasShield,
            };

            this.broadcast(hs.toJson(
                "gamedata",
                JSON.stringify(gamedata)
            ))
        });
    }

    skillselected(ws, type, index) {

        if (--ws.selectCount < 0) return;

        ws.skills.push({ type, index });

        if (type == 3 || type == 4 || type == 5) {
            ws.abliSkills.push({ type: type, index: index });
        }

        this.broadcast(hs.toJson(
            "skillselected",
            JSON.stringify({
                id: ws.id,
                type: type,
                skill: index
            })
        ));

        if (--this.skillSelectCount <= 0) {
            let pos = new Vector2(-4.0, 0.0);

            this.sendGamedata();
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

    applyStat(ws) {
        this.resetPlayerValue(ws);
        ws.abliSkills?.forEach(x => {
            let inst = new skills(null, ws).skills[x.type][x.index]();
            ws.damage = inst.damage;
            ws.hp = inst.hp;
            ws.maxhp = inst.hp;
            ws.blocksize = inst.blocksize;
            ws.blockspeed = inst.blockspeed;
            ws.speed = inst.speed;
            ws.ratefire = inst.ratefire;
            ws.rotationspeed = inst.rotationspeed;
            ws.pushpower = inst.pushpower;
            ws.bomb = inst.bomb;
            ws.penetrate = inst.penetrate;
            ws.hasShield = inst.hasShield;
        });
    }

    damage(damagedws) {
        let attackws = this.players.find(x => x != damagedws);
        let sk = new skills();
        let damage;

        if (!attackws.neutralized) {
            attackws.skills
                ?.filter(x => x.type == 0)
                ?.forEach(skill => {
                    sk = new skills(sk, attackws)
                        .skills[0][skill.index]();
                });
        }
        
        damage = sk.damage;
        
        if (!damagedws.neutralized) {
            damagedws.skills
                ?.filter(x => x.type == 2)
                ?.forEach(skill => {
                    sk = new skills(sk, damagedws)
                        .skills[2][skill.index](damage);
                });
        }
            
        damage = sk.damage;

        if (!attackws.neutralized) {
            attackws.skills
                ?.filter(x => x.type == 1)
                ?.forEach(skill => {
                    sk = new skills(sk, attackws)
                        .skills[1][skill.index](damage);
                });
        }
 
        if (damagedws.hasShield) { // 쉴드
            damagedws.hasShield = false;
            sk.hpReturn = 0;
            damage = 0;

            this.broadcast(hs.toJson(
                "skill", JSON.stringify({ command: "shieldoff" })
            ));

            damagedws.shieldTimeout = setTimeout(() => {
                damagedws.hasShield = true;
                this.broadcast(hs.toJson(
                    "skill", JSON.stringify({ command: "shieldon"})
                ));
            }, 7000);
        }
        
        attackws.hp += sk.hpReturn;
        damagedws.hp -= damage;

        if (sk.knockout) { // 기절
            damagedws.knockedout = true;
            damagedws.knockedoutTimeout = setTimeout(() => {
                damagedws.knockedout = false;
            }, 500);
        }
        
        if (sk.neutralize) { // 무력화
            damagedws.neutralized = true;
            damagedws.neutralizedTimeout = setTimeout(() => {
                damagedws.neutralized = false;
            }, 1500);
        }

        if (damagedws.hp <= 0) {
            this.dead(damagedws);
            return;
        }
        
        this.broadcast(hs.toJson(
            "damage",
            JSON.stringify({
                id: damagedws.id,
                maxhp: damagedws.maxhp,
                hp: damagedws.hp,
                atkhp: attackws.hp,
                atkmaxhp: attackws.maxhp,
                specialCommands: sk.specialCommands,
            })
        ));
    }
    
    dead(deadws) {
        ++this.deadPlayers;
        this.justDiedPlayer = deadws;
        deadws.hp = deadws.maxhp; // FIXME: 임시
        deadws.knockedout = false;
        deadws.neutralized = false;

        clearImmediate(deadws.shieldTimeout);
        clearImmediate(deadws.knockedoutTimeout);
        clearImmediate(deadws.neutralizedTimeout);

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
                        wonId: ws.id,
                        setWon: ws.setWon
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
            ws.knockedout = false;
            ws.neutralized = false;
            ws.invincible = false;
            clearImmediate(ws.shieldTimeout);
            clearImmediate(ws.knockedoutTimeout);
            clearImmediate(ws.neutralizedTimeout);
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