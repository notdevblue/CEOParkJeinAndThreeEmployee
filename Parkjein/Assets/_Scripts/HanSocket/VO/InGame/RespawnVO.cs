using System;
using UnityEngine;

namespace HanSocket.VO.InGame
{
    [Serializable]
    public class RespawnVO : ValueObject
    {
        public int id;
        public Vector2 pos;
    }
}