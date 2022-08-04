using System.Collections;
using System.Collections.Generic;
using HanSocket.Data;
using HanSocket.VO.InGame;
using UI.InGame;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class GameEndHandler : HandlerBase
    {
        protected override string Type => "gameend";
        private GameEndCanvas _gameEndCvs;
        private GameEndVO vo;

        private void Start()
        {
            _gameEndCvs = FindObjectOfType<GameEndCanvas>();
            _gameEndCvs.gameObject.SetActive(false);
        }

        protected override void OnArrived(string payload)
        {
            vo = JsonUtility.FromJson<GameEndVO>(payload);
        }

        protected override void OnFlag()
        {
            string displayText =
                (vo.winnerId == UserData.Instance.myId ? "승리\n" : "패배\n")
                + vo.reason;

            _gameEndCvs.Display(displayText);
        }
    }
}