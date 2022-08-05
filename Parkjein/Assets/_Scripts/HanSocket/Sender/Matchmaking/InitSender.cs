using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HanSocket.Sender.Matchmaking
{
    public class InitSender : MonoBehaviour
    {
        IEnumerator Start()
        {
            yield return null;

            WebSocketClient.Instance.Send("init", "");
        }
    }
}