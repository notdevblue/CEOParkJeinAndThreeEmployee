const hs = require("../HanSocket/HanSocket");
const game = require("./game");
const write = require("./Logger");

class match
{
    constructor() {
        this.minimumPlayer = 2;
        this.matches = [];
        this.matchId = 0;
        this.matchqueue = [];
    }

    inQueue(ws) {
        ws.match = true;
        this.matchqueue.push(ws);
        hs.send(ws, hs.toJson("match", ""));

        this.matchmake();
    }

    leaveQueue(ws) {
        ws.match = false;
        let idx = this.matchqueue.findIndex(x => x == ws);
        this.matchqueue.splice(idx, (idx != -1)); // 브렌치리스
        
        hs.send(ws, hs.toJson("unmatch", ""));
    }


    matchmake() {
        if (this.matchqueue.length < this.minimumPlayer) return;

        let queue = [];

        for (let i = 0; i < this.minimumPlayer; ++i) {
            let idx = Math.floor(Math.random() * this.matchqueue.length);
            queue.push(this.matchqueue[idx]);
            
            this.matchqueue.splice(idx, 1);
        }

        if (queue.findIndex(x => x == undefined) != -1) {
            write("메치메이킹 중 유저 처리 오류 발생");
            return;
        }

        this.matches[++this.matchId] = new game(queue, this.matchId);

        queue.forEach(ws => {
            ws.match = false;
            ws.game  = this.matches[this.matchId];
            hs.send(ws, hs.toJson("ingame", ""));
        });
    }


    
}


module.exports = new match();