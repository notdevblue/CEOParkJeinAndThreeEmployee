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
        private int _selectCount = 0;

        private void Awake()
        {
            okButton.onClick.AddListener(() => {
                // TODO: 나중에 Payload 로 선택한 스킬 인덱스 보내야 함
                WebSocketClient.Instance.Send("skillselected", "");
            });
        }

        public void Set(bool canSelect, int selectCount = 1)
        {
            okButton.interactable = canSelect;
            _selectCount = selectCount;

            gameObject.SetActive(true);
        }
    }
}