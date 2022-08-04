using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanSocket.Handlers.Matchmaking
{
    public class InGameHandler : HandlerBase
    {
        protected override string Type => "ingame";

        protected override void OnArrived(string payload)
        {
            Debug.Log("메치메이킹 완료");
        }

        protected override void OnFlag()
        {
            
        }
    }
}