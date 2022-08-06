using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using HanSocket.VO.InGame;
using HanSocket.Data;

namespace HanSocket.Handlers.InGame
{
    public class SkillHandler : HandlerBase
    {
        protected override string Type => "skill";

        ConcurrentQueue<SpecialCommands> vos
            = new ConcurrentQueue<SpecialCommands>();

        protected override void OnArrived(string payload)
        {
            vos.Enqueue(JsonUtility.FromJson<SpecialCommands>(payload));
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
                    Debug.LogError(vo.command);

                    if(vo.command.Equals("shieldoff"))
                    {
                        GameObject obj = UserData.Instance.users[vo.id];

                        if(obj != null)
                        {
                            EffectManager.Instance.PlayEffect("shield", obj.transform.position, Vector2.zero, true, 0.5f, obj.transform);
                        }
                    }
                }
            }
        }
    }
}