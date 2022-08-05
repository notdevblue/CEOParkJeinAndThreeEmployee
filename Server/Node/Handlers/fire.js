const hs = require("../HanSocket/HanSocket");
const write = require("../Utils/Logger");

module.exports = {
    type: "fire",
    handle: (ws, data) => {
        if (ws.game == null) {
            write("fire: 게임 중이 아닌 클라이언트의 요청", ws.ipAddr);
            return;
        }

        ws.game.broadcast(hs.toJson("fire", data), [ws.id]);
    }
};