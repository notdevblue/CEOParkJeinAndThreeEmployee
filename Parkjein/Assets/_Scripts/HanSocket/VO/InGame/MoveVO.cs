using UnityEngine;
using System;

namespace HanSocket.VO.InGame
{    
    [Serializable]
    public class MoveVO : ValueObject
    {
        public int id;
        public Vector2 pos;

        public MoveVO(int id, Vector2 pos)
        {
            this.id  = id;
            this.pos = pos;
        }
    }
}