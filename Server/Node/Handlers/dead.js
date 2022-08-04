const write = require("../Utils/Logger");

module.exports = {
    type: "dead",
    handle: (ws, data) => {
        if (ws.game == null) {
            write("dead: 게임 중이 아닌 클라이언트의 요청", ws.ipAddr);
            return;
        }

        ws.game.dead(ws);
    }
};