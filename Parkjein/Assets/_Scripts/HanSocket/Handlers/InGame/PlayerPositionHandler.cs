using System.Collections.Concurrent;
using UnityEngine;
using HanSocket.VO.InGame;
using HanSocket.Data;

namespace HanSocket.Handlers.InGame
{
    public class PlayerPositionHandler : HandlerBase
    {
        protected override string Type => "move";

        ConcurrentQueue<MoveVO> vos
            = new ConcurrentQueue<MoveVO>();

        protected override void OnArrived(string payload)
        {
            vos.Enqueue(JsonUtility.FromJson<MoveVO>(payload));
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
                    if (!UserData.Instance.users.ContainsKey(vo.id))
                    {
                        Debug.LogError($"Cannot find user: ${vo.id}");
                        return;
                    }

                    UserData.Instance.users[vo.id]
                        .GetComponent<Remote>()
                        .SetTarget(vo.pos);
                }
            }
        }
    }
}