using System;
using System.Collections.Generic;

namespace HanSocket.VO.InGame
{
    [Serializable]
    public class GameEndVO : ValueObject
    {
        public int winnerId;
        public string reason;
        public List<WinVO> winList;
    }
}