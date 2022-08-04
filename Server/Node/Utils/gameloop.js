let activeLoops = [];
const getLoopId = (function () {
   let staticLoopId = 0;
   return function () {
      return staticLoopId++;
   }
})();

module.exports.setGameLoop = function (update, tickLength = 1000 / 30) {
   let lastUpdate = Date.now();
   return activeLoops.push(setInterval(() => {
      let now = Date.now();
      let deltaTime = 1.0 / (now - lastUpdate);
      lastUpdate = now;

      update(deltaTime);
   }, tickLength));
}

module.exports.clearGameLoop = (id) => {
   clearInterval(id);
}