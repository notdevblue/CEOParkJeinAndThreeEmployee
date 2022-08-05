using System.Collections.Concurrent;
using HanSocket.Data;
using HanSocket.VO.InGame;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class DamageHandler : HandlerBase
    {
        protected override string Type => "damage";

        ConcurrentQueue<DamageVO> vos
            = new ConcurrentQueue<DamageVO>();

        protected override void OnArrived(string payload)
        {
            vos.Enqueue(JsonUtility.FromJson<DamageVO>(payload));
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
                    GameObject obj = UserData.Instance.users[vo.id];

                    Debug.LogWarning($"Damaged: {vo.id}, HP: {vo.hp}");
                }
            }
        }
    }
}