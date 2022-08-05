using UnityEngine;
using System;

namespace HanSocket.VO.InGame
{
    [Serializable]
    public class BulletStopVO : ValueObject
    {
        public Vector2 pos;
        public Quaternion rot;

        public BulletStopVO(Vector2 pos, Quaternion rot)
        {
            this.pos = pos;
            this.rot = rot;
        }
    }
}