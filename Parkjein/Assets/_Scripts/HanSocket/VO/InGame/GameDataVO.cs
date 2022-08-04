using System;
using System.Collections.Generic;

namespace HanSocket.VO.InGame
{    
    [Serializable]
    public class GameDataVO : ValueObject
    {
        public List<int> players;
        public int myId;

        public float hp;
        public float speed;
        public float jumpPower;
        public float blockSize;
        public float blockRateFire;
        public float blockSpeed;
        public float rotationSpeed;
    }
}