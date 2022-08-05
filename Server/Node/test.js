const skills = require("./Skills/skills.js");

test("skill test", () => {
    expect(new skills(null).ability[0]().damage).toBe(30)
});
