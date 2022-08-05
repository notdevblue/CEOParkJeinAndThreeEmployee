class skills {

    constructor(instance) {
        if (instance == null) {
            this.damage          = 20;
            this.hp              = 100;
            this.speed           = 1;
            this.jumpPower       = 5;
            this.blocksize       = 5;
            this.ratefire        = 0.5;
            this.blockspeed      = 1;
            this.rotationspeed   = 1;
            this.pushpower       = 10;
            this.hpReturn        = 0;
            this.specialCommands = [];
            this.longLifeUsed    = false;
            
        } else {
            console.log(instance);

            this.damage        = instance.damage;
            this.hp            = instance.hp;       
            this.speed         = instance.speed;
            this.jumpPower     = instance.jumpPower;
            this.blocksize     = instance.blocksize;     
            this.ratefire      = instance.ratefire;    
            this.blockspeed    = instance.blockspeed;
            this.rotationspeed = instance.rotationspeed;
            this.pushpower     = instance.pushpower;
            
            this.hpReturn        = instance.hpReturn;
            this.specialCommands = instance.specialCommands;
            this.longLifeUsed    = instance.longLifeUsed;
        }
        



        this.attack = [
            () => { // 흡혈
                return new skills(this);
            },
            () => { // 기절
                this.specialCommands.push("knockout");
                return new skills(this);
            },
            () => { // 무력화
                this.specialCommands.push("neutralize");
                return new skills(this);
            },
            () => { // 크리티컬
                if (Math.random() >= 0.6) {
                    this.damage *= 2.0;
                }
                return new skills(this);
            }
        ]

        this.defend = [
            (damage) => { // 끈질긴 생명력
                this.damage = damage;

                if (!this.longLifeUsed
                    && (damage - this.hp <= 0)) {
                    this.damage = this.hp - 1;
                    this.longLifeUsed = true;
                }

                return new skills(this);
            },
            (damage) => { // 쉴드
                return new skills(this);
            },
            (damage) => { // 단단한 신체
                return new skills(this);
            }
        ]

        this.ability = [
            () => { // 큰 블록
                this.damage     += 10;
                this.blocksize  += 3;
                this.blockspeed -= 0.5;

                return new skills(this);
            },
            () => { // 작은 블록
                this.damage     -= 6;
                this.blocksize  -= 2;
                this.blockspeed += 0.5;

                return new skills(this);
            },
            () => { // 건강한 신체
                this.hp += 50;

                return new skills(this);
            },
            () => { // 빠른 속도
                this.damage   -= 4;
                this.speed    += 0.5;
                this.ratefire -= 0.2;
                
                return new skills(this);
            },
            () => { // 빠른 회전
                this.blockspeed    += 1;
                this.rotationspeed += 1;

                return new skills(this);
            },
            () => { // 넉백
                this.damage    -= 4;
                this.knockback += 10;

                return new skills(this);
            }
        ]

        this.attackTransform = [
            () => { // 관통
                this.specialCommands("penetrate");
                return new skills(this);
            },
            () => { // 폭탄
                this.specialCommands("bomb");
                return new skills(this);
            }
        ]
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