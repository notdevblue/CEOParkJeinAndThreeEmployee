using UnityEngine;
using System;

namespace HanSocket.VO
{
   [Serializable]
   public abstract class ValueObject
   {
      public string ToJson()
         => JsonUtility.ToJson(this);
   }
}