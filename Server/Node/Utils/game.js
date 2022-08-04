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
        this.gameWonStackToWin    = 3;
        this.setWonStackToWin     = 2;
        
        // (3판 2선승제) => 5판 3선승
        // 한 세트 끝 마다 능력
    }

    broadcast(payload, excludeIds = [-1,]) {
        this.players.forEach(ws => {

            // 제외
            if (excludeIds.findIndex(x => x == ws.id) != -1)
                return;
            
            ws.send(payload);
            ws.onDamage = [];

            this.init(ws);
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

        this.broadcast(hs.toJson(
            "newloop",
            JSON.stringify({
                skill: this.justDiedPlayer.id,
            }),
        ));

        this.justDiedPlayer = null;
    }

    loaded() {
        ++this.loadedCount;

        // 모든 클라이언트가 로딩 완료된 경우
        if (this.loadedCount >= this.players.length) {

            let ids = [];
            this.players.forEach(ws => {
                ids.push(ws.id);
            });

            ws.setWon  = 0;
            ws.gameWon = 0;

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
                ws.send(hs.toJson(
                    "gamedata",
                    JSON.stringify(inGameData)
                ));
            });
        }
        
        this.skillSelectCount = 2;
        this.newLoop();
    }

    skillselected(index) {
        // TODO: 스킬 적용

        if (--this.skillSelectCount <= 0) {
            let pos = new Vector2(-2.0, 0.0);

            this.players.forEach(ws => {
                ws.send(hs.toJson(
                    "gamestart",
                    JSON.stringify({
                        pos: pos,
                    }),
                ));
                pos.x = 2;
            });
        }
        
    }
    
    dead(deadws) {
        ++this.deadPlayers;
        this.justDiedPlayer = deadws;
        if (this.deadPlayers >= this.players.length - 1) {
            let ws = this.players.find(x => x != deadws);

            if (++ws.setWon >= this.setWonStackToWin) {
                if (++ws.gameWon >= this.gameWonStackToWin) {
                    this.gameEnd(ws.id);
                    return;
                }

                this.newLoop();
            } else {
                // TODO: 부활 처리
                hs.send(deadws, hs.toJson(
                    "respawn",
                    JSON.stringify({
                        pos: new Vector2(0.0, 0.0),
                    }),
                ));
            }
        }
            
        
        this.skillSelectCount = 1;
        this.newLoop();
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

            ws.game = null;
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
            this.gameEnd(lastPlayerid.id, "상대가 게임을 종료했습니다.");
        }

        leftws.game = null;
    }

}

module.exports = game;