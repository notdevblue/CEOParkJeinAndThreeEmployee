using System;

namespace HanSocket.VO
{
   [Serializable]
   public class DataVO : ValueObject
   {
      public string type;
      public string payload;

      public DataVO(string type, string payload)
      {
         this.type = type;
         this.payload = payload;
      }
   }
}