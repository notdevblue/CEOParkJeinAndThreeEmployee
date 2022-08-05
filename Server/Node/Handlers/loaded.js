const write = require("../Utils/Logger");

module.exports = {
    type: "loaded",
    handle: (ws, data) => {
        if (ws.game == null) {
            write("메치 이루어지지 않은 클라이언트의 요청", ws.ipAddr);
            return;
        }

        ws.game.loaded(ws);
    }
};