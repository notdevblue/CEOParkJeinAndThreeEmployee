const hs = require("../HanSocket/HanSocket");
const write = require("../Utils/Logger");

module.exports = {
    type: "move",
    handle: (ws, data) => {
        if (ws.game == null) {
            write("move: 게임 중이 아닌 클라이언트의 요청", ws.ipAddr);
            return;
        }

        if (ws.knockedout) return;

        ws.game.players.forEach(s => {
            if (s.id != ws.id)
                s.send(hs.toJson("move", data));
        });
    }
};