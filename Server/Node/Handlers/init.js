const hs = require("../HanSocket/HanSocket");

module.exports = {
    type: "init",
    handle: (ws, data) => {
        hs.send(ws, hs.toJson(
            "init",
            JSON.stringify({ id: ws.id })
        ));
    }
};