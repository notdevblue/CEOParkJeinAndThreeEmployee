using UnityEngine;
using System;
using System.Collections.Generic;

namespace HanSocket.VO.InGame
{
    [Serializable]    
    public class DamageVO : ValueObject
    {
        public int id;
        public float hp;
        public float maxhp;
        public float atkhp;
        public float atkmaxhp;
        public Vector2 point;
        public List<SpecialCommands> specialCommands;

        public DamageVO(Vector2 point)
        {
            this.point = point;
        }
    }

    [Serializable]
    public class SpecialCommands
    {
        public int id;
        public string command;
        public float param;
    }
}