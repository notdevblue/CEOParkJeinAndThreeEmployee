using System;
using HanSocket;
using HanSocket.VO.InGame;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Managers;

namespace UI.InGame
{    
    public class SkillButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        public Button btnSelect;
        public Image skillImage;
        public TMP_Text text;

        [SerializeField]
        private int _skillType;
        [SerializeField]
        private int _skillIndex;

        private Action _onClickCallback;

        private Sprite backSprite;
        private Sprite frontSprite;

        private void Awake()
        {
            btnSelect.onClick.AddListener(() => {
                WebSocketClient.Instance
                    .Send(
                        "skillselected",
                        JsonUtility.ToJson(new SkillVO(_skillType, _skillIndex))
                    );

                btnSelect.interactable = false;

                SoundManager.Instance.PlaySelect();
                _onClickCallback();
            });
        }


        public void Init(bool canSelect, int type, int index, Action onClickCallback)
        {
            _onClickCallback       = onClickCallback;
            btnSelect.interactable = canSelect;

            _skillType  = type;
            _skillIndex = index;

            var obj   = SkillImageSetter.Instance.Get(type, index);
            text.text = obj.text;

            backSprite = skillImage.sprite = obj.backSprite;
            frontSprite = obj.sprite;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            skillImage.sprite = frontSprite;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            skillImage.sprite = backSprite;
        }
    }
}