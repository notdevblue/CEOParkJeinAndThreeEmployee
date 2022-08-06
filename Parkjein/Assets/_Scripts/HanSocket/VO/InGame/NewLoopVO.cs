using System;
using System.Collections.Generic;

namespace HanSocket.VO.InGame
{
    [Serializable]
    public class NewLoopVO : ValueObject
    {
        // 스킬 선택할 플레이어 아이디
        public int skill;
        public int selectCount;
        public List<SkillVO> skillList;
    }

    [Serializable]
    public class SkillVO : ValueObject
    {
        public int id;
        public int type;
        public int skill;

        public SkillVO(int type, int skill)
        {
            this.type = type;
            this.skill = skill;
        }
    }
}