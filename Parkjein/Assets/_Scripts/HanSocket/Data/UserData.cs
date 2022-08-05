using System.Collections.Generic;
using System.Linq;
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
            bool isLeftUI = false;

            vo.players.ForEach(e => {

                var obj = MonoBehaviour.Instantiate(prefab);
                obj.AddComponent<User>().id = e;
                obj.SetActive(false);

                PlayerData data = obj.GetComponent<PlayerData>();

                data.InitValue(
                        vo.jumpPower,
                        vo.speed,
                         vo.blockSpeed,
                        vo.blockRateFire,
                        vo.rotationSpeed
                    );

                if (e != myId)
                {
                    obj.name = $"RemotePlayer {e}";

                    isLeftUI = myId > e ? false : true;
                    obj.GetComponent<PlayerMove>().enabled = false;
                    obj.GetComponent<PlayerShoot>().enabled = false;
                    MonoBehaviour.Destroy(obj.GetComponent<PositionSender>());
                    MonoBehaviour.Destroy(obj.GetComponent<Rigidbody2D>());
                    MonoBehaviour.Destroy(obj.transform.Find("ME").gameObject);
                }
                else
                {
                    obj.name = $"Player {e}";

                    obj.GetComponent<Rigidbody2D>().gravityScale = 1;
                    obj.GetComponent<Remote>().enabled = false;
                }
                users.Add(e, obj);
            });

            List<PlayerUI> uis = MonoBehaviour.FindObjectsOfType<PlayerUI>().ToList();
            Debug.Log(uis.Count);
            vo.players.ForEach(e =>
            {
                if (e != myId)
                {
                    users[e].GetComponent<PlayerData>().MyUI = uis.Find(x => x.gameObject.name == (isLeftUI ? "Right" : "Left"));
                }
                else
                {
                    users[e].GetComponent<PlayerData>().MyUI = uis.Find(x => x.gameObject.name == (isLeftUI ? "Left" : "Right"));
                }
            });
        }
    }
}