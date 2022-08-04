using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanSocket.Sender.InGame
{
    public class LoadedSender : MonoBehaviour
    {
        private void Start()
        {
            WebSocketClient.Instance.Send("loaded", "");
        }
    }
}