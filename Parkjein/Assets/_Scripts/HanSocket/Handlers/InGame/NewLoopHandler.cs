using System.Collections;
using System.Collections.Generic;
using HanSocket.Data;
using HanSocket.VO.InGame;
using UI.InGame;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class NewLoopHandler : HandlerBase
    {
        protected override string Type => "newloop";

        private NewLoopVO vo;

        private SkillSelectCanvas _cvsSkillSelect;

        private void Start()
        {
            _cvsSkillSelect = FindObjectOfType<SkillSelectCanvas>();
            _cvsSkillSelect.gameObject.SetActive(false);
        }

        protected override void OnArrived(string payload)
        {
            vo = JsonUtility.FromJson<NewLoopVO>(payload);
        }

        protected override void OnFlag()
        {
            _cvsSkillSelect
                .Set((vo.skill == UserData.Instance.myId) || (vo.skill == -1));
        }
    }
}