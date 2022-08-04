using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanSocket.VO.InGame
{
    public class FireVO : ValueObject
    {
        public int bulletIdx;
        public Vector2 startPos;
        public Vector2 dir;
        public float bulletSpeed;

        public FireVO(int bulletIdx,Vector2 startPos, Vector2 dir, float bulletSpeed)
        {
            this.bulletIdx = bulletIdx;
            this.startPos = startPos;
            this.dir = dir;
            this.bulletSpeed = bulletSpeed;
        }
    }
}