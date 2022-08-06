const hs = require("../HanSocket/HanSocket");

module.exports = {
    type: "bulletstop",
    handle: (ws, data) => {
        if (ws.game == null) return;

        ws.game.players.forEach(x => {
            if (x.id == ws.id) return;

            x.send(hs.toJson("bulletstop", data));
        })
    }
};