const hs = require("../HanSocket/HanSocket");

class game
{
    constructor(players, id) {
        this.players = players;
        this.id = id;

        this.loadedCount = 0;
        this.deadPlayers = 0;

        this.initialHp = 100;
        this.initialAtk = 10;
        this.initialSpeed = 5;
    }

    broadcast(payload, excludeIds = [-1,]) {
        this.players.forEach(ws => {

            // 제외
            if (excludeIds.findIndex(x => x == ws.id) != -1)
                return;

            hs.send(ws, payload);
            ws.won = 0;
            ws.onDamage = [];

            this.init(
                ws,
                this.initialHp,
                this.initialAtk,
                this.initialSpeed
            );
        });
    }

    init(ws, hp, atk, speed) {
        ws.hp = hp
        ws.atk = atk;
        ws.speed = speed;
    }

    newLoop() {
        this.players.forEach(ws => {
            this.deadPlayers = 0;
        });
    }

    loaded() {
        ++this.loadedCount;

        // 모든 클라이언트가 로딩 완료된 경우
        if (this.loadedCount >= this.players.length) {

            let ids = [];
            this.players.forEach(ws => {
                ids.push(ws.id);
            });

            let inGameData = {
                // 플레이어 아이디 데이터
                players: ids,

                // 자신 아이디
                myId: -1,

                // 기본값
                hp: this.initialHp,
                atk: this.initialAtk,
                speed: this.initialSpeed,
            };

            this.players.forEach(ws => {
                inGameData.myId = ws.id;
                ws.send(hs.toJson(
                    "gamestart",
                    JSON.stringify(inGameData)
                ));
            });
        }
    }
    
    dead(deadws) {
        ++this.deadPlayers;
        if (this.deadPlayers >= this.players.length - 1)
            ++this.players.find(x => x != deadws).won;
        
        this.newLoop();
    }
    
    gameEnd(winnerId, reason) {
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