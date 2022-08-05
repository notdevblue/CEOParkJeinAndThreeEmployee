module.exports = {
    type: "damage",
    handle: (ws, data) => {
        if (ws.game == null) {
            write("damage: 게임 중이 아닌 클라이언트의 요청", ws.ipAddr);
            return;
        }

        ws.game.damage(ws);
    }
};