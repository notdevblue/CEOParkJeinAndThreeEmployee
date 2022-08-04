using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HanSocket;

namespace UI.InGame
{
    public class SkillSelectCanvas : MonoBehaviour
    {
        public Button okButton;

        private void Awake()
        {
            okButton.onClick.AddListener(() => {
                // TODO: 나중에 Payload 로 선택한 스킬 인덱스 보내야 함
                WebSocketClient.Instance.Send("skillselected", "");
                this.gameObject.SetActive(false);
            });
        }

        public void Set(bool canSelect)
        {
            okButton.interactable = canSelect;
            gameObject.SetActive(true);
        }
    }
}