const hs = require("../HanSocket/HanSocket");
const Vector2 = require("./Vector2");

class game
{
    constructor(players, id) {
        this.players = players;
        this.id = id;

        this.justDiedPlayer;

        this.loadedCount = 0;
        this.deadPlayers = 0;

        this.initialHp            = 100;
        this.initialAtk           = 20;
        this.initialSpeed         = 1;
        this.initialJump          = 5;
        
        this.initialblockSize     = 5;
        this.initialBlockRateFire = 0.5;
        this.initialBlockSpeed    = 1;
        this.initialRotationSpeed = 1;
        
        this.initialPushPower     = 10; // 피격 시
        
        this.skillSelectCount     = 0; // 스킬 선택 카운트
        this.lastLostPlayerId     = -1; // 마지막으로 (세트)진 플레이어 아이디
        this.gameWonStackToWin    = 3;
        this.setWonStackToWin     = 2;
        this.invincibleTimeMs     = 5000;

        // (3판 2선승제) => 5판 3선승
        // 한 세트 끝 마다 능력
    }

    broadcast(payload, excludeIds = [-1,]) {
        this.players.forEach(ws => {

            // 제외
            if (excludeIds.findIndex(x => x == ws.id) != -1)
                return;
            
            hs.send(ws, payload);
        });
    }

    init(
        ws,
        atk           = this.initialAtk,
        hp            = this.initialHp,
        speed         = this.initialSpeed,
        jump          = this.initialJump,
        blocksize     = this.initialblockSize,
        blockRateFire = this.blockRateFire,
        blockspeed    = this.initialBlockSpeed,
        rotationspeed = this.initialRotationSpeed,
        pushpower     = this.initialPushPower) {
        
        ws.hp            = hp
        ws.atk           = atk;
        ws.speed         = speed;
        ws.jump          = jump;
        ws.blocksize     = blocksize;
        ws.blockRateFire = blockRateFire;
        ws.blockspeed    = blockspeed;
        ws.rotationspeed = rotationspeed;
        ws.pushpower     = pushpower;
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

    loaded() {
        ++this.loadedCount;

        // 모든 클라이언트가 로딩 완료된 경우
        if (this.loadedCount >= this.players.length) {

            let ids = [];
            this.players.forEach(ws => {
                this.init(ws);

                ws.onDamage    = [];
                ws.setWon      = 0;
                ws.gameWon     = 0;
                ws.invincible  = false;
                ws.selectCount = 1;

                ids.push(ws.id);
            });


            let inGameData = {
                // 플레이어 아이디 데이터
                players: ids,

                // 자신 아이디
                myId: -1,

                // 기본값
                hp: this.initialHp,
                
                // 이동
                speed:      this.initialSpeed,
                jumpPower:  this.initialJump,
                
                // 블럭 관련
                blockSize:      this.initialblockSize,
                blockRateFire:  this.initialBlockRateFire,
                blockSpeed:     this.initialBlockSpeed,
                rotationSpeed:  this.initialRotationSpeed,
            };

            this.players.forEach(ws => {
                inGameData.myId = ws.id;
                hs.send(ws, hs.toJson(
                    "gamedata",
                    JSON.stringify(inGameData)
                ));
            });

            this.skillSelectCount = 2;
            this.newLoop();
        }
        

    }

    skillselected(ws, index) {
        // TODO: 스킬 적용

        if (--ws.selectCount < 0) return;

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