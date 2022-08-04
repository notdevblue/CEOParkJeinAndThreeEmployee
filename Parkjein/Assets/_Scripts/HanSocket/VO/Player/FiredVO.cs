using System;
using System.Collections.Generic;
using UnityEngine;

namespace HanSocket.VO.Player
{   
   [Serializable]
   public class FiredVO : ValueObject
   {
      public int id = -1;
      public Vector2 firedPos;
      public List<float> angles = new List<float>();
      public float speed;
      public float alivefor;

      /// <summary>
      /// type: fired
      /// </summary>
      public FiredVO(int id, Vector2 firedPos, List<float> angles)
      {
         this.id = id;
         this.firedPos = firedPos;
         this.angles = angles;
      }
   }
}