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
        public List<SpecialCommands> specialCommands;
    }

    [Serializable]
    public class SpecialCommands
    {
        public int id;
        public string command;
        public float param;
    }
}