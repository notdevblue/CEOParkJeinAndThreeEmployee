using System;
using System.Collections.Generic;

namespace HanSocket.VO.Queue
{
   [Serializable]
   public class RoomDataVO : ValueObject
   {
      public List<JoinedVO> userlist;

      /// <summary>
      /// type: roomdata
      /// </summary>
      public RoomDataVO(List<JoinedVO> userlist)
      {
         this.userlist = userlist;
      }
   }
}