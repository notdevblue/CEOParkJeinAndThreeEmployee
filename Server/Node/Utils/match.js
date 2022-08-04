const hs = require("../HanSocket/HanSocket");
const write = require("./Logger");

class match
{
    constructor() {
        this.minimumPlayer = 1;
        this.matches;
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

        this.queue = [];

        for (let i = 0; i < this.minimumPlayer; ++i) {
            let idx = Math.floor(Math.random() * this.matchqueue.length);
            this.queue.push(this.matchqueue[idx]);
            
            this.matchqueue.splice(idx, 1);
        }

        if (this.queue.findIndex(x => x == undefined) != -1)
        {
            write("메치메이킹 중 유저 처리 오류 발생");
            return;
        }

        this.queue.forEach(ws => {
            ws.ingame = true;
            hs.send(ws, hs.toJson("ingame", ""));
        })
    }


    
}


module.exports = new match();