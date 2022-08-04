using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanSocket.Handlers.Matchmaking
{
    public class UnMatchHandler : HandlerBase
    {
        protected override string Type => "unmatch";

        protected override void OnArrived(string payload)
        {
            Debug.Log("메치메이킹 나감");
        }

        protected override void OnFlag()
        {
            
        }
    }
}