const SkillVO = require("../VO/SkillVO");

class skills {

    constructor(instance, ws) {
        if (instance == null) {
            this.damage = ws == null ? 20 : ws.damage;
            this.speed = ws == null ? 4 : ws.speed;
            this.jumpPower = 5;
            this.blocksize = ws == null ? 5 : ws.blocksize;
            this.ratefire = ws == null ? 0.25 : ws.ratefire;
            this.blockspeed = ws == null ? 5 : ws.blockspeed;
            this.rotationspeed = ws == null ? 7 : ws.rotationspeed;
            this.pushpower = ws == null ? 10 : ws.pushpower;
            
            this.hpReturn = 0;
            this.specialCommands = [];
            this.neutralize = false;
            this.knockout = false;
            this.penetrate = false;
            this.bomb = false;

        } else {
            this.damage = instance.damage;
            this.speed = instance.speed;
            this.jumpPower = instance.jumpPower;
            this.blocksize = instance.blocksize;
            this.ratefire = instance.ratefire;
            this.blockspeed = instance.blockspeed;
            this.rotationspeed = instance.rotationspeed;
            this.pushpower = instance.pushpower;
            
            this.hpReturn = instance.hpReturn;
            this.specialCommands = instance.specialCommands;
            this.neutralize = instance.neutralize;
            this.knockout = instance.knockout;
            this.penetrate = isntance.penetrate;
            this.bomb = isntance.bomb;
        }

        this.hp = ws == null ? 100 : ws.hp;


        this.atk = [
            // 공격
            () => { // 크리티컬
                if (Math.random() >= 0.6) {
                    this.damage *= 2.0;
                }
                return this;
            },
            () => { // 기절
                this.specialCommands.push(
                    new SkillVO("knockout", 0.5)
                );
                this.knockout = true;

                return this;
            },
            () => { // 무력화
                this.specialCommands.push(
                    new SkillVO("neutralize", 1.5)
                );
                this.neutralize = true;

                return this;
            },
        ]

        this.atkPas = [
            (damage) => { // 흡혈
                this.hpReturn += damage / 2.0;
                return this;
            },
        ]

        this.def = [
            // 방어
            (damage) => { // 단단한 신체
                if (Math.random() >= 0.6) {
                    damage /=  2.0;
                }
                this.damage = damage;

                return this;
            },
            (damage) => { // 끈질긴 생명력
                if (damage - this.hp <= 0) {
                }
                this.damage = hp - 1;

                return this;
            },
        ]

        this.defPas = [ // 페시브 방어 스킬
            (damage) => { // 쉴드
                return this;
            },
        ]

        this.abil = [
            // 능력치
            () => { // 큰 블록
                this.damage     += 10;
                this.blocksize  += 3;
                this.blockspeed -= 0.5;

                return this;
            },
            () => { // 작은 블록
                this.damage     -= 6;
                this.blocksize  -= 2;
                this.blockspeed += 0.5;

                return this;
            },
            () => { // 건강한 신체
                this.hp += 50;

                return this;
            },
            () => { // 빠른 속도
                this.damage   -= 4;
                this.speed    += 0.5;
                this.ratefire -= 0.2;
                
                return this;
            },
            () => { // 빠른 회전
                this.blockspeed    += 1;
                this.rotationspeed += 1;

                return this;
            },
            () => { // 넉백
                this.damage    -= 4;
                this.pushpower += 10;

                return this;
            },
        ]

        this.atkTrm = [
            // 변형
            () => { // 관통
                this.specialCommands.push(
                    new SkillVO("penetrate", 0)
                );
                this.penetrate = true;
                return this;
            },
            () => { // 폭탄
                this.specialCommands.push(
                    new SkillVO("bomb", 10.0)
                );
                this.bomb = true;
                return this;
            }
        ]

        this.skills = [
            this.atk,
            this.atkPas,
            this.def,
            this.defPas,
            this.abil,
            this.atkTrm
        ];
    }

    calculate() {
        return {
            damage: this.damage,
            hp: this.hp,
            speed: this.speed,
            jumpPower: this.jumpPower,
            blocksize: this.blocksize,
            ratefire: this.ratefire,
            blockspeed: this.blockspeed,
            rotationspeed: this.rotationspeed,
            pushpower: this.pushpower,
            hpReturn: this.hpReturn,
            specialCommands: this.specialCommands,
        }
    }
}

module.exports = skills;