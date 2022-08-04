using System;
using UnityEngine;

namespace HanSocket.VO.Player
{
   [Serializable]
   public class MoveVO : ValueObject
   {
      public Vector2 delta;
      public bool run = false;

      /// <summary>
      /// type: move
      /// </summary>
      public MoveVO(Vector2 delta)
      {
         this.delta = delta;
      }
   }
}