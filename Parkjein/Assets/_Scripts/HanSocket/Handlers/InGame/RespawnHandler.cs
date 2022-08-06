using System.Collections.Concurrent;
using UnityEngine;
using HanSocket.VO.InGame;
using HanSocket.Data;
using HanSocket.Sender.InGame;
using UnityEngine.Events;

namespace HanSocket.Handlers.InGame
{
    public class RespawnHandler : HandlerBase
    {
        public UnityEvent<bool> OnDead
            = new UnityEvent<bool>();
            
        protected override string Type => "respawn";
        public DeadSender deadSender;

        private ConcurrentQueue<RespawnVO> vos
            = new ConcurrentQueue<RespawnVO>();

        protected override void OnArrived(string payload)
        {
            vos.Enqueue(JsonUtility.FromJson<RespawnVO>(payload));
        }

        protected override void OnFlag()
        {
        }

        private void Update()
        {
            while (vos.Count > 0)
            {
                if (vos.TryDequeue(out var vo))
                {
                    GameObject user = UserData.Instance.users[vo.id];
                    GameObject wUser = UserData.Instance.users[vo.wonId];
                    Rigidbody2D rigid = user.GetComponent<Rigidbody2D>();

                    if (rigid != null)
                        rigid.velocity = Vector2.zero;

                    user.GetComponent<Remote>()
                        ?.SetTarget(vo.pos);
                        
                    user.transform.position = vo.pos;
                    user.SetActive(true);

                    if(vo.id == WebSocketClient.Instance.id)
                    {
                        deadSender.respawned = true;
                        EffectManager.Instance.EnableDampingEndFrame();
                    }

                    wUser.GetComponent<PlayerData>().MyUI.SetWinImg(vo.setWon);
                    user.GetComponent<PlayerData>().MyUI.SetHp(1.0f);

                    BulletPool.Instance.InitBullet();

                    OnDead?.Invoke(vo.id == WebSocketClient.Instance.id);
                }
            }
        }
    }
}