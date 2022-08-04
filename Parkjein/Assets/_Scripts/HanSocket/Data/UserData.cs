using System.Collections.Generic;
using HanSocket.VO.InGame;

namespace HanSocket.Data
{
    public class UserData : Singleton<UserData>
    {
        public int myId;

        // 공격
        public int atk;

        // 이동
        public float speed;

        // 채력
        public int maxHp;
        public int curHp;

        // 유저 아이디 모아둔 것들
        public List<int> users;


        public void Init(GameStartVO vo)
        {
            myId  = vo.myId;
            atk   = vo.atk;
            speed = vo.speed;
            maxHp = vo.hp;
            curHp = vo.hp;

            users = vo.players;
        }
    }
}