using System.Collections.Concurrent;
using UnityEngine;
using HanSocket.VO.InGame;
using HanSocket.Data;

namespace HanSocket.Handlers.InGame
{
    public class BulletstopHandler : HandlerBase
    {
        protected override string Type => "bulletstop";

        private ConcurrentQueue<BulletStopVO> vos
         = new ConcurrentQueue<BulletStopVO>();

        protected override void OnArrived(string payload)
        {
            vos.Enqueue(JsonUtility.FromJson<BulletStopVO>(payload));
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
                    
                    TetrisBullet bullet = BulletPool.Instance.GetActiveBullet(vo.id, vo.shooterId);
                    bullet.transform.position = vo.pos;
                    bullet.transform.rotation = vo.rot;

                }
            }
        }

    }
}