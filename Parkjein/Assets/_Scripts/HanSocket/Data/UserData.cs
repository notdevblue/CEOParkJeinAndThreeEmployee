using System.Collections.Generic;
using System.Linq;
using HanSocket.Sender.InGame;
using HanSocket.VO.InGame;
using UnityEngine;

namespace HanSocket.Data
{
    public class UserData : Singleton<UserData>
    {
        public int myId {
            get => WebSocketClient.Instance.id;
        }

        // 유저 아이디: 유저 오브젝트
        public Dictionary<int, GameObject> users
            = new Dictionary<int, GameObject>();

        public void Init(GameDataVO vo, GameObject prefab)
        {
            int id = vo.id;

            float speed = vo.speed;
            float jumpPower = vo.jumpPower;

            float maxHp = vo.hp;
            float curHp = vo.hp;

            float blockSize          = vo.blockSize;
            float blockSpeed         = vo.blockSpeed;
            float blockRotationSpeed = vo.rotationSpeed;
            float blockRateFire      = vo.blockRateFire;

            bool isLeftUI = false;            

            bool alreadyadded = users.ContainsKey(id);
            GameObject obj;

            if (!alreadyadded)
            {
                obj = MonoBehaviour.Instantiate(prefab);
                obj.AddComponent<User>().id = vo.id;
                obj.SetActive(false);
                users.Add(id, obj);
            }
            else
            {
                obj = users[id];
            }

            PlayerData data = obj.GetComponent<PlayerData>();

            data.InitValue(
                vo.jumpPower,
                vo.speed,
                vo.blockSpeed,
                vo.blockRateFire,
                vo.rotationSpeed
            );

            if (!alreadyadded)
            {

                if (id != myId)
                {
                    obj.name = $"RemotePlayer {id}";

                    isLeftUI = myId > id ? false : true;
                    obj.GetComponent<PlayerMove>().enabled = false;
                    obj.GetComponent<PlayerShoot>().enabled = false;
                    MonoBehaviour.Destroy(obj.GetComponent<PositionSender>());
                    MonoBehaviour.Destroy(obj.GetComponent<Rigidbody2D>());
                    MonoBehaviour.Destroy(obj.transform.Find("ME").gameObject);
                }
                else
                {
                    obj.name = $"Player {id}";

                    obj.GetComponent<Rigidbody2D>().gravityScale = 1;
                    obj.GetComponent<Remote>().enabled = false;
                }
            }

            List<PlayerUI> uis = MonoBehaviour.FindObjectsOfType<PlayerUI>().ToList();
            if (id != myId)
            {
                users[id].GetComponent<PlayerData>().MyUI = uis.Find(x => x.gameObject.name == (isLeftUI ? "Right" : "Left"));
            }
            else
            {
                users[id].GetComponent<PlayerData>().MyUI = uis.Find(x => x.gameObject.name == (isLeftUI ? "Left" : "Right"));
            }
        }
    }
}