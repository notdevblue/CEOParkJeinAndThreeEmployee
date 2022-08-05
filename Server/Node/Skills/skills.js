const SkillVO = require("../VO/SkillVO");

class skills {

    constructor(instance, ws) {
        if (instance == null) {
            this.damage = 20;
            this.hp = 100;
            this.speed = 1;
            this.jumpPower = 5;
            this.blocksize = 5;
            this.ratefire = 0.5;
            this.blockspeed = 1;
            this.rotationspeed = 1;
            this.pushpower = 10;
            
            this.hpReturn = 0;
            this.specialCommands = [];

        } else {
            this.damage = instance.damage;
            this.hp = ws.hp;
            this.speed = instance.speed;
            this.jumpPower = instance.jumpPower;
            this.blocksize = instance.blocksize;
            this.ratefire = instance.ratefire;
            this.blockspeed = instance.blockspeed;
            this.rotationspeed = instance.rotationspeed;
            this.pushpower = instance.pushpower;
            
            this.hpReturn = instance.hpReturn;
            this.specialCommands = instance.specialCommands;
        }

        this.atk = [
            // 공격
            () => { // 크리티컬
                if (Math.random() >= 0.6) {
                    this.damage *= 2.0;
                }
                return new skills(this, ws);
            },
            (damage) => { // 흡혈
                this.hpReturn += damage / 2.0;
                return new skills(this, ws);
            },
            () => { // 기절
                this.specialCommands.push(
                    JSON.stringify(new SkillVO("knockout", 0.5))
                );
                return new skills(this, ws);
            },
            () => { // 무력화
                this.specialCommands.push(
                    JSON.stringify(new SkillVO("neutralize", 1.5))
                );
                return new skills(this, ws);
            },
        ]

        this.def = [
            // 방어
            (damage) => { // 단단한 신체
                if (Math.random() >= 0.6) {
                    damage = damage / 2.0;
                }
                this.hp -= damage;

                return new skills(this, ws);
            },
            (damage) => { // 끈질긴 생명력
                if (damage - this.hp <= 0) {
                    this.hp = 1;
                }

                return new skills(this, ws);
            },
        ]

        this.defPas = [ // 페시브 방어 스킬
            (damage) => { // 쉴드
                return new skills(this, ws);
            },
        ]

        this.abil = [
            // 능력치
            () => { // 큰 블록
                this.damage     += 10;
                this.blocksize  += 3;
                this.blockspeed -= 0.5;

                return new skills(this, ws);
            },
            () => { // 작은 블록
                this.damage     -= 6;
                this.blocksize  -= 2;
                this.blockspeed += 0.5;

                return new skills(this, ws);
            },
            () => { // 건강한 신체
                this.hp += 50;

                return new skills(this, ws);
            },
            () => { // 빠른 속도
                this.damage   -= 4;
                this.speed    += 0.5;
                this.ratefire -= 0.2;
                
                return new skills(this, ws);
            },
            () => { // 빠른 회전
                this.blockspeed    += 1;
                this.rotationspeed += 1;

                return new skills(this, ws);
            },
            () => { // 넉백
                this.damage    -= 4;
                this.knockback += 10;

                return new skills(this, ws);
            },
        ]

        this.atkTrm = [
            // 변형
            () => { // 관통
                this.specialCommands.push(
                    JSON.stringify(new SkillVO("penetrate", null))
                );
                return new skills(this, ws);
            },
            () => { // 폭탄
                this.specialCommands.push(
                    JSON.stringify(new SkillVO("bomb", 10.0))
                );
                return new skills(this, ws);
            }
        ]

        this.skills = [
            this.atk,
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