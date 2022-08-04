using System;
using System.Collections.Generic;

namespace HanSocket.VO.InGame
{
    [Serializable]
    public class NewLoopVO : ValueObject
    {
        // 스킬 선택할 플레이어 아이디
        public int skill;
    }
}