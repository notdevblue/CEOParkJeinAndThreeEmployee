using System;

namespace HanSocket.VO.Player
{
   [Serializable]
   public class HitVO : ValueObject
   {
      public int id;
      public int hp;

      /// <summary>
      /// type: id
      /// </summary>
      public HitVO(int id, int hp)
      {
         this.id = id;
         this.hp = hp;
      }
   }
}