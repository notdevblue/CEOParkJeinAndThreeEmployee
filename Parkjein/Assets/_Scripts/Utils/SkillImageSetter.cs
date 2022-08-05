using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SkillImageSetter : MonoSingleton<SkillImageSetter>
{
    public List<SkillImage> skillImages;

    public SkillImage Get(int type, int skill)
    {
        return skillImages
            .Find(x => (x.type == type) && (x.skill == skill));
    }
    
}


[Serializable]
public class SkillImage
{
    public int type;
    public int skill;
    public Sprite sprite;
    public string text;
}