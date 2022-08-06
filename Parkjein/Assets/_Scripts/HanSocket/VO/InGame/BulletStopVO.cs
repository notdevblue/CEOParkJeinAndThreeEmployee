using UnityEngine;
using System;

namespace HanSocket.VO.InGame
{
    [Serializable]
    public class BulletStopVO : ValueObject
    {
        public int id;
        public int shooterId;
        public Vector2 pos;
        public Quaternion rot;

        public BulletStopVO(int id, int shooterId, Vector2 pos, Quaternion rot)
        {
            this.id = id;
            this.shooterId = shooterId;
            this.pos = pos;
            this.rot = rot;
        }
    }
}