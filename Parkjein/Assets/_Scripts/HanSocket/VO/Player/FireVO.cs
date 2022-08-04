using System;
using UnityEngine;

namespace HanSocket.VO.Player
{   
   [Serializable]
   public class FireVO
   {
      public float angle = 0.0f;
      public Vector2 firedPos = Vector2.zero;

      /// <summary>
      /// type: fire
      /// </summary>
      public FireVO(float angle, Vector2 firedPos)
      {
         this.angle = angle;
         this.firedPos = firedPos;
      }
   }
}