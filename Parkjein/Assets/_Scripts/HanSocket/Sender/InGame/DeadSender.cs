using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;

namespace HanSocket.Sender.InGame
{
    public class DeadSender : MonoBehaviour, IEventable
    {
        public void Active(GameObject other)
        {
            if (!other.GetComponent<Remote>().enabled)
                WebSocketClient.Instance.Send("dead", "");
        }

        public void Deactive(GameObject other) { }
    }
}