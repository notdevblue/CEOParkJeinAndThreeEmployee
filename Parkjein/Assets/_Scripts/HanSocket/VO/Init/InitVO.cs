using System;

namespace HanSocket.VO
{
   [Serializable]
   public class InitVO : ValueObject
   {
      public int id;

      /// <summary>
      /// type: init
      /// </summary>
      public InitVO(int id)
      {
         this.id = id;
      }
   }
}