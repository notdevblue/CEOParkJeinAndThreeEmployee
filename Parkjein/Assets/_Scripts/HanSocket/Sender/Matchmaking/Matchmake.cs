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
        public TMP_Text text;
        private bool onMatch = false;

        private void Start()
        {
            matchButton.onClick.AddListener(() => {
                onMatch = !onMatch;
                WebSocketClient.Instance.Send("matchmaking", "");
                SoundManager.Instance.PlaySelect();
                text.text = !onMatch ? "Match" : "Unmatch";
            });
        }
    }
}