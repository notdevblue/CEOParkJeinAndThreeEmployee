using System;
using System.Collections.Generic;

namespace HanSocket.VO.InGame
{    
    [Serializable]
    public class GameStartVO : ValueObject
    {
        public List<int> players;
        public int myId;
        public int hp;
        public int atk;
        public float speed;
    }
}