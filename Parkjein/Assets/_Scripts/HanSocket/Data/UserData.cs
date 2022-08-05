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
                obj.AddComponent<User>().id = e;
                obj.SetActive(false);

                PlayerMove move = obj.GetComponent<PlayerMove>();
                PlayerShoot shoot = obj.GetComponent<PlayerShoot>();

                if (e != myId)
                {
                    obj.name = $"RemotePlayer {e}";

                    move.enabled = false;
                    shoot.enabled = false;
                    MonoBehaviour.Destroy(obj.GetComponent<PositionSender>());
                    MonoBehaviour.Destroy(obj.GetComponent<Rigidbody2D>());
                    MonoBehaviour.Destroy(obj.GetComponent<BoxCollider2D>());
                }
                else
                {
                    obj.name = $"Player {e}";

                    move.InitValue(
                        vo.jumpPower,
                        vo.speed
                    );

                    shoot.InitValue(
                        vo.blockSpeed,
                        vo.blockRateFire,
                        vo.rotationSpeed
                    );

                    obj.GetComponent<Rigidbody2D>().gravityScale = 1;
                    obj.GetComponent<Remote>().enabled = false;
                    
                    // InitValue
                }

                users.Add(e, obj);
            });
        }
    }
}