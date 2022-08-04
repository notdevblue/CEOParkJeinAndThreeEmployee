using System;

namespace HanSocket.VO.Player
{   
   [Serializable]
   public class DeadVO : ValueObject
   {
      public int id;
      public int killedby;

      /// <summary>
      /// type: dead
      /// </summary>
      public DeadVO(int id, int killedby)
      {
         this.id = id;
         this.killedby = killedby;
      }
   }
}