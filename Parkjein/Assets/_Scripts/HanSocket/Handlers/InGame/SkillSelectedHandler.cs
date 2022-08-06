using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using HanSocket.VO.InGame;

namespace HanSocket.Handlers.InGame
{
    public class SkillSelectedHandler : HandlerBase
    {
        protected override string Type => "skillselected";

        private ConcurrentQueue<SkillVO> vos
            = new ConcurrentQueue<SkillVO>();

        protected override void OnArrived(string payload)
        {
            vos.Enqueue(JsonUtility.FromJson<SkillVO>(payload));
        }

        protected override void OnFlag()
        {
        }


        private void Update()
        {
            while (vos.Count > 0)
            {
                if (vos.TryDequeue(out var vo))
                {
                    Debug.Log($"{vo.id}: {vo.type} 의 {vo.skill} 선택");
                }
            }
        }
    }
}