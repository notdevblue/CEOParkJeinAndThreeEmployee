using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HanSocket;
using HanSocket.VO.InGame;
using DG.Tweening;

namespace UI.InGame
{
    public class SkillSelectCanvas : MonoBehaviour
    {
        private int _selectCount = 0;
        private CanvasGroup _cvsGroup;

        private SkillButton[] _skillIcon;

        public float fadeDuration = 1.0f;


        private void Awake()
        {
            _skillIcon = GetComponentsInChildren<SkillButton>();
            _cvsGroup = GetComponent<CanvasGroup>();
        }

        public void Set(bool canSelect, List<SkillVO> skills, int selectCount = 1)
        {
            _selectCount = selectCount;

            for (int i = 0; i < _skillIcon.Length; ++i)
            {
                _skillIcon[i].Init(
                    canSelect,
                    skills[i].type,
                    skills[i].skill,
                    () => {
                    }
                );
            }

            gameObject.SetActive(true);
            DoAlpha();
        }

        private void OnDisable()
        {
            _cvsGroup.alpha = 0.0f;
        }

        public void DoAlpha()
        {
            _cvsGroup.DOFade(1.0f, 1.0f).SetEase(Ease.InOutQuad);
        }
    }
}