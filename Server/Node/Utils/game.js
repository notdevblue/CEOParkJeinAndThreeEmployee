const hs = require("../HanSocket/HanSocket");

class game
{
    constructor(players, id) {
        this.players = players;
        this.id = id;

        this.loadedCount = 0;

        this.initialHp = 100;
        this.initialAtk = 10;
        this.initialSpeed = 5;
    }

    broadcast(payload) {
        this.players.forEach(ws => {
            hs.send(ws, payload);
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
}

module.exports = game;