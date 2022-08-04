using System;
using UnityEngine;

namespace HanSocket.VO.Queue
{
   [Serializable]
   public class JoinedVO : ValueObject
   {
      public int id;
      public int hp;
      public string nickname;
      public Vector2 pos;

      /// <summary>
      /// type; joiend
      /// </summary>
      public JoinedVO(int id, string nickname)
      {
         this.id = id;
         this.nickname = nickname;
      }
   }
}