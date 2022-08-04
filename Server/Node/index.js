require("dotenv").config();

const hs = require("./HanSocket/HanSocket.js");
const write = require("./Utils/Logger.js");
const match = require("./Utils/match.js");

hs.process(ws => {

}, ws => {
    if (ws.match == true)
        match.leaveQueue(ws);
    if (ws.game != null)
        ws.game.left(ws);
});