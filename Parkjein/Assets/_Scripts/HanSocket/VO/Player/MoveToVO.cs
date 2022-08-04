using System;
using UnityEngine;

namespace HanSocket.VO.Player
{
   [Serializable]
   public class MoveToVO : ValueObject
   {
      public int id;
      public Vector2 pos;

      /// <summary>
      /// type: moveto
      /// </summary>
      public MoveToVO(int id, Vector2 pos)
      {
         this.id = id;
         this.pos = pos;
      }
   }
}