using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HanSocket.VO.InGame;
using HanSocket.Data;

namespace HanSocket.Handlers.InGame
{
    public class PlayerPositionHandler : HandlerBase
    {
        protected override string Type => "move";

        private MoveVO vo;

        protected override void OnArrived(string payload)
        {
            vo = JsonUtility.FromJson<MoveVO>(payload);
        }

        protected override void OnFlag()
        {
            UserData.Instance.users[vo.id].GetComponent<Remote>();
        }
    }
}