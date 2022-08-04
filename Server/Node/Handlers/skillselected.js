module.exports = {
    type: "skillselected",
    handle: (ws, data) => {
        if (ws.game == null) {
            write("skillselected: 게임 중이 아닌 클라이언트의 요청", ws.ipAddr);
            return;
        }

        ws.game.skillselected(-1);
    }
};