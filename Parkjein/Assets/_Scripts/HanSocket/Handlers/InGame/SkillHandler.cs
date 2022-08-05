using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using HanSocket.VO.InGame;

namespace HanSocket.Handlers.InGame
{
    public class SkillHandler : HandlerBase
    {
        protected override string Type => "skill";

        ConcurrentQueue<string> vos
            = new ConcurrentQueue<string>();

        protected override void OnArrived(string payload)
        {
            vos.Enqueue(JsonUtility.FromJson<string>(payload));
        }

        protected override void OnFlag()
        {
        }

        private void Update()
        {
            while(vos.Count > 0)
            {
                if (vos.TryDequeue(out var vo))
                {
                    Debug.LogError(vo);
                }
            }
        }
    }
}