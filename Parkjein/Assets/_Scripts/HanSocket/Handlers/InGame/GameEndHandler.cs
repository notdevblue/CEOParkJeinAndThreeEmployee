using System.Collections;
using System.Collections.Generic;
using HanSocket.Data;
using HanSocket.Sender.InGame;
using HanSocket.VO.InGame;
using UI.InGame;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class GameEndHandler : HandlerBase
    {
        protected override string Type => "gameend";
        private GameEndCanvas _gameEndCvs;
        private MainPanel mainPanel;
        private GameEndVO vo;
        
        private void Start()
        {
            _gameEndCvs = FindObjectOfType<GameEndCanvas>();
            mainPanel = FindObjectOfType<MainPanel>();
            _gameEndCvs.gameObject.SetActive(false);
        }

        protected override void OnArrived(string payload)
        {
            vo = JsonUtility.FromJson<GameEndVO>(payload);
        }

        protected override void OnFlag()
        {
            int leftScore = 0, rightScore = 0;
            var sender = FindObjectsOfType<PositionSender>();
            for (int i = 0; i < sender.Length; ++i)
                sender[i].Stop();

            string displayText =
                (vo.winnerId == WebSocketClient.Instance.id ? "Victory\n" : "Lost\n")
                + vo.reason;

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
            });

            mainPanel.SetScoreText(leftScore, rightScore);

            _gameEndCvs.Display(displayText);

            UserData.Instance.GameEnd();
        }
    }
}