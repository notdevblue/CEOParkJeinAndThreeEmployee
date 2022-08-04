using System.Collections.Generic;
using HanSocket.Sender.InGame;
using HanSocket.VO.InGame;
using UnityEngine;

namespace HanSocket.Data
{
    public class UserData : Singleton<UserData>
    {
        public int myId;

        // 이동
        public float speed;
        public float jump;

        // 채력
        public float maxHp;
        public float curHp;

        // 블록 관련
        public float blockSize;
        public float blockSpeed;
        public float blockRotationSpeed;
        public float blockRateFire;

        // 유저 아이디: 유저 오브젝트
        public Dictionary<int, GameObject> users;


        public void Init(GameDataVO vo, GameObject prefab)
        {
            myId  = vo.myId;

            speed = vo.speed;

            maxHp = vo.hp;
            curHp = vo.hp;

            blockSize          = vo.blockSize;
            blockSpeed         = vo.blockSpeed;
            blockRotationSpeed = vo.rotationSpeed;
            blockRateFire      = vo.blockRateFire;

            users = new Dictionary<int, GameObject>();
            vo.players.ForEach(e => {

                var obj = MonoBehaviour.Instantiate(prefab);
                obj.SetActive(false);
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