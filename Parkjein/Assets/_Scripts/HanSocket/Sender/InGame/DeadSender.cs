using System.Collections;
using System.Collections.Generic;
using HanSocket.Data;
using Objects;
using UnityEngine;

namespace HanSocket.Sender.InGame
{
    public class DeadSender : MonoBehaviour, IEventable
    {
        public bool respawned = true;

        public void Active(GameObject other)
        {
            if (respawned && other.GetComponent<User>().id == WebSocketClient.Instance.id)
            {
                respawned = false;
                WebSocketClient.Instance.Send("dead", "");
            }
        }

        public void Deactive(GameObject other) { }
    }
}