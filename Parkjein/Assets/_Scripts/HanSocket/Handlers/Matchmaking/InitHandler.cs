using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HanSocket.VO;

namespace HanSocket.Handlers.Matchmaking
{
    public class InitHandler : HandlerBase
    {
        protected override string Type => "init";

        protected override void OnArrived(string payload)
        {
            WebSocketClient.Instance.id =
                JsonUtility.FromJson<InitVO>(payload).id;
        }

        protected override void OnFlag()
        {
            
        }
    }
}