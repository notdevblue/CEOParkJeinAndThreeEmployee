using System.Collections;
using System.Collections.Generic;
using HanSocket.Data;
using HanSocket.VO.InGame;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class NewLoopHandler : HandlerBase
    {
        protected override string Type => "newloop";

        private NewLoopVO vo;

        protected override void OnArrived(string payload)
        {
            vo = JsonUtility.FromJson<NewLoopVO>(payload);
        }

        protected override void OnFlag()
        {
            foreach(var item in UserData.Instance.users)
            {
                item.Value.transform.position = Vector2.zero;
            }
        }
    }
}