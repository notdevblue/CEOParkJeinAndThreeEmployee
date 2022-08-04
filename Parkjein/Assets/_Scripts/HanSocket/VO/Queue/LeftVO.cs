using System;

namespace HanSocket.VO.Queue
{
   [Serializable]
   public class LeftVO : ValueObject
   {
      public int id;

      /// <summary>
      /// type: left
      /// </summary>
      public LeftVO(int id)
      {
         this.id = id;
      }
   }
}