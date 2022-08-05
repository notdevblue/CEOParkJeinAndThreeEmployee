using UnityEngine;
using System;

namespace HanSocket.VO.InGame
{
    [Serializable]    
    public class DamageVO : ValueObject
    {
        public int id;
        public float hp;
    }
}