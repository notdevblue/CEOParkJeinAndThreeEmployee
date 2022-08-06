using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;

namespace HanSocket.Sender.Matchmaking
{
    public class Matchmake : MonoBehaviour
    {
        public Button matchButton;

        private void Start()
        {
            matchButton.onClick.AddListener(() => {
                WebSocketClient.Instance.Send("matchmaking", "");
                SoundManager.Instance.PlaySelect();
            });
        }
    }
}