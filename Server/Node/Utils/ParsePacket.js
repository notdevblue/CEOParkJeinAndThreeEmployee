const write = require("./Logger");

function parse(packet, ipAddr) {
   try {
      return JSON.parse(packet);
   } catch (e) {
      write(`[EE] Error parsing packet from ${ipAddr}. packet: ${packet}`);
      return null;
   }
};

module.exports = parse;