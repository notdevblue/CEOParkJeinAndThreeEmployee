using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanSocket.Sender.InGame
{
    public class DeadSender : MonoBehaviour
    {
        public void OnDead()
        {
            Debug.LogError("ASDASDASD");
            WebSocketClient.Instance.Send("dead", "");
        }
    }
}