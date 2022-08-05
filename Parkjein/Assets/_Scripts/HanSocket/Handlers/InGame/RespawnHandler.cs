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

                    user.GetComponent<Remote>()
                        .SetTarget(vo.pos);
                    user.GetComponent<Rigidbody2D>()
                        .velocity = Vector2.zero;
                    user.transform.position = vo.pos;
                    user.SetActive(true);
                }
            }
        }
    }
}