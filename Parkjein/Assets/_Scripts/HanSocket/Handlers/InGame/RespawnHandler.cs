using System.Collections.Concurrent;
using UnityEngine;
using HanSocket.VO.InGame;
using HanSocket.Data;

namespace HanSocket.Handlers.InGame
{
    public class RespawnHandler : HandlerBase
    {
        protected override string Type => "respawn";

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
                    user.GetComponent<PlayerSetUI>().MyUI.SetHp(1);
                        
                    user.transform.position = vo.pos;
                    user.SetActive(true);

                    wUser.GetComponent<PlayerSetUI>().MyUI.SetWinImg(vo.setWon);
                }
            }
        }
    }
}