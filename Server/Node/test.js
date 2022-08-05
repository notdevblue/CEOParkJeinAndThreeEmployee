const skills = require("./Skills/skills.js");

let ws;

beforeEach(() => {
    ws = {};
    ws.hp = 100;
});

test("skill test", () => {
    expect(new skills(null, ws).skills[3][0]().damage).toBe(30)
});
