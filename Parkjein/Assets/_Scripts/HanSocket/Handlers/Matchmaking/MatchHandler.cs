using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanSocket.Handlers.Matchmaking
{
    public class MatchHandler : HandlerBase
    {
        protected override string Type => "match";

        protected override void OnArrived(string payload)
        {
            Debug.Log("메치메이킹 대기 중");
        }

        protected override void OnFlag()
        {
            
        }
    }
}