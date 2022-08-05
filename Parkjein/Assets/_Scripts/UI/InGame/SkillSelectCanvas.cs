using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HanSocket;
using HanSocket.VO.InGame;

namespace UI.InGame
{
    public class SkillSelectCanvas : MonoBehaviour
    {
        private int _selectCount = 0;
        
        private SkillButton[] _skillIcon;


        private void Awake()
        {
            _skillIcon = GetComponentsInChildren<SkillButton>();
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
                    if (--_selectCount <= 0)
                            gameObject.SetActive(false);
                    }
                );
            }

            gameObject.SetActive(true);
        }
    }
}