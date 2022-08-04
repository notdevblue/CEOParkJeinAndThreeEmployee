using System;
using System.Collections.Generic;

namespace HanSocket.VO.InGame
{    
    [Serializable]
    public class GameStartVO : ValueObject
    {
        public List<int> players;
        public int myId;

        public float atk;
        public float hp;
        public float speed;
        public float jumpPower;
        public float blockSize;
        public float blockWeight;
        public float rotationSpeed;
    }
}