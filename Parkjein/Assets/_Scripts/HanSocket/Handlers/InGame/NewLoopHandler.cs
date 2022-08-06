using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HanSocket.Data;
using HanSocket.VO.InGame;
using UI.InGame;
using UnityEngine;
using Managers;

namespace HanSocket.Handlers.InGame
{
    public class NewLoopHandler : HandlerBase
    {
        protected override string Type => "newloop";

        private NewLoopVO vo;

        private SkillSelectCanvas _cvsSkillSelect;
        [SerializeField]
        private MainPanel mainPanel;

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
            int leftScore = 0, rightScore = 0;

            _cvsSkillSelect
                .Set(
                    (vo.skill == WebSocketClient.Instance.id) || (vo.skill == -1),
                    vo.skillList,
                    vo.selectCount
                );

            SoundManager.Instance.PlaySelectSkillBgm();
            BulletPool.Instance.InitBullet();

            vo.winList?.ForEach(x =>
            {
                PlayerData data = UserData.Instance.users[x.id].GetComponent<PlayerData>();

                if (data == null || data.MyUI == null) return;

                if (data.MyUI.isLeft)
                {
                    leftScore = x.win;
                }
                else
                {
                    rightScore = x.win;
                }
                data.MyUI.NewLoop();
            });

            mainPanel.SetScoreText(leftScore, rightScore);
        }
    }
}