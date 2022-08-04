const write = require("../Utils/Logger");
const match = require("../Utils/match");

module.exports = {
    type: "matchmaking",
    handle: (ws, data) => {

        if (ws.match) // 메치 메이킹 중이라면 나감
            match.leaveQueue(ws);
        else // 메치 메이킹 중이 아니라면 들어감
            match.inQueue(ws);
    }
};