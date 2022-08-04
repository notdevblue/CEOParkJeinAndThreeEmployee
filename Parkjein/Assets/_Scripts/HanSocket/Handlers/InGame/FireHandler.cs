using System.Collections.Concurrent;
using UnityEngine;
using HanSocket.VO.InGame;
using HanSocket.Data;

namespace HanSocket.Handlers.InGame
{
    public class FireHandler : HandlerBase
    {
        protected override string Type => "fire";

        private FireVO fireVO;

        protected override void OnArrived(string payload)
        {
            fireVO = JsonUtility.FromJson<FireVO>(payload);
        }

        protected override void OnFlag()
        {
            BulletPool.Instance.GetBullet(fireVO.bulletIdx).Shoot(fireVO.startPos, fireVO.dir, fireVO.bulletSpeed);
        }

    }
}