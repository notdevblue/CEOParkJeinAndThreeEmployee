using System.Collections.Generic;
using HanSocket.Sender.InGame;
using HanSocket.VO.InGame;
using UnityEngine;

namespace HanSocket.Data
{
    public class UserData : Singleton<UserData>
    {
        public int myId;

        // 공격
        public float atk;

        // 이동
        public float speed;

        // 채력
        public float maxHp;
        public float curHp;

        // 유저 아이디: 유저 오브젝트
        public Dictionary<int, GameObject> users;


        public void Init(GameStartVO vo, GameObject prefab)
        {
            myId  = vo.myId;
            atk   = vo.atk;
            speed = vo.speed;
            maxHp = vo.hp;
            curHp = vo.hp;

            users = new Dictionary<int, GameObject>();
            vo.players.ForEach(e => {

                var obj = MonoBehaviour.Instantiate(prefab);
                if (e != myId)
                {
                    obj.name = $"RemotePlayer {e}";
                    obj.AddComponent<Remote>();
                }
                else
                {
                    obj.name = $"Player {e}";
                    obj.AddComponent<PlayerMove>();
                    obj.AddComponent<PlayerShoot>();
                    obj.AddComponent<PositionSender>();
                    obj.AddComponent<Rigidbody2D>();
                }

                users.Add(e, obj);
            });
        }
    }
}