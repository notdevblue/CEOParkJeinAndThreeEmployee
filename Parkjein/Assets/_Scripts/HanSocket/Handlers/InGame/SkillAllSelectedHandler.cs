using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class SkillAllSelectedHandler : HandlerBase
    {
        protected override string Type => "skillallselected";

        protected override void OnArrived(string payload)
        {
        }

        protected override void OnFlag()
        {
            Debug.Log("와 스킬 다 선택했데요");
        }
    }
}