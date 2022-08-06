const write = require("../Utils/Logger");

module.exports = {
    type: "skillselected",
    handle: (ws, data) => {
        if (ws.game == null) {
            write("skillselected: 게임 중이 아닌 클라이언트의 요청", ws.ipAddr);
            return;
        }

        const payload = JSON.parse(data);

        ws.game.skillselected(ws, payload.type, payload.skill);
    }
};