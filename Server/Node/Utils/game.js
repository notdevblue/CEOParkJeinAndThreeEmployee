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
        
        const skill = new skills();

        let randomSkills = [];
        let sendList     = [];

        // for (let i = 0; i < skill.skills.length; ++i) {
        //     randomSkills.push([]);
        //     for (let j = 0; j < skill.skills[i].length; ++j) {
        //         randomSkills[i].push(j);
        //     }
        // }

        skill.skills.forEach(x => {
            x.forEach(y => {
                randomSkills.push(randomSkills.length);
            });
        });

        console.log(randomSkills);
        
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

        /*
        3 : 0  : 3  : 0 1 2
        1 : 3  : 4  : 0
        2 : 4  : 6  : 0 1
        1 : 6  : 7  : 0 
        6 : 7  : 13 : 0 1 2 3 4 5
        2 : 13 : 15 : 0 1
        */

        // this.justDiedPlayer.skills.forEach(e => {
        //     e.type;
        //     e.skill;
        // });
        
        // for (let i = 0; i < 5; ++i) {
        //     let idx1 = Math.floor(Math.random() * (randomSkills.length - 1));
        //     let idx2 = Math.floor(Math.random() * (randomSkills[idx1].length - 1));

        //     sendList.push({
        //         type: idx1,
        //         skill: randomSkills[idx1].splice(idx2, 1)[0]
        //     });

        //     if (randomSkills[idx1].length <= 0) {
        //         delete randomSkills[idx1];
        //     }
        // }

        // TODO:
        // 스킬 픽 구현 중
        // 아레쪽에 데미지 스킬 적용시켜서 하는거 테스트 안함
        // random 으로 skill 중에 하나 뽑고
        // skill[random] 에서 하나 또 뽑고

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
        
        ws.setWon      = 0;
        ws.gameWon     = 0;
        ws.invincible  = false;
        ws.selectCount = 1;
        ws.hp          = 100;
        ws.maxhp       = 100;
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
        let atkinstance = new skills(null, attackws);
        let definstance = new skills(null, damagedws);
        let damage;

        attackws.skills
            ?.filter(x => x.type == 0)
            ?.forEach(skill => {
                atkinstance =
                    new skills(atkinstance, attackws)
                        .skills[0][skill.index]();
            });
        
        damage = atkinstance.damage;

        damagedws.skills
            ?.filter(x => x.type == 2)
            ?.forEach(skill => {
                definstance
                    = new skills(definstance, damagedws)
                        .skills[1][skill.index](damage);
                damage -= definstance.damage;
            });
        
        attackws.skills
            ?.find(x => x.type == 2)
            ?.forEach(skill => {
                atkinstance =
                    new skills(atkinstance, attackws)
                        .skills[2][skill.index]();
            });
        
        attackws.hp += atkinstance.hpReturn;
        damagedws.hp = definstance.hp;

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
                specialCommands: atkinstance.specialCommands,
            })
        ));
    }
    
    dead(deadws) {
        ++this.deadPlayers;
        this.justDiedPlayer = deadws;
        deadws.hp = 100; // FIXME: 임시

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