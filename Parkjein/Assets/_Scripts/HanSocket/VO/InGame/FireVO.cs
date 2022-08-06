using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanSocket.VO.InGame
{
    public class FireVO : ValueObject
    {
        public int shooterId;
        public int bulletId;

        public int bulletIdx;
        public Vector2 startPos;
        public Vector2 dir;
        public float bulletSpeed;
        public float rotationSpeed;

        public FireVO(
            int shooterId,
            int bulletId,
            int bulletIdx,
            Vector2 startPos,
            Vector2 dir,
            float bulletSpeed,
            float rotationSpeed)
        {
            this.shooterId = shooterId;
            this.bulletId = bulletId;
            this.bulletIdx = bulletIdx;
            this.startPos = startPos;
            this.dir = dir;
            this.bulletSpeed = bulletSpeed;
            this.rotationSpeed = rotationSpeed;
        }
    }
}