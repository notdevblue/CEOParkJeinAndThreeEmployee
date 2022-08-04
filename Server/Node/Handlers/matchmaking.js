const write = require("../Utils/Logger");
const match = require("../Utils/match");

module.exports = {
    type: "matchmaking",
    handle: (ws, data) => {
        if (ws.game != null) {
            write("match: 인게임 들어가 있는 클라에게서 온 메치 요청.", ws.ipAddr);
            return;
        }

        if (ws.match) // 메치 메이킹 중이라면 나감
            match.leaveQueue(ws);
        else // 메치 메이킹 중이 아니라면 들어감
            match.inQueue(ws);
    }
};