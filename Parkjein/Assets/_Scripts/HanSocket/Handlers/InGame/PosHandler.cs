using System.Collections;
using System.Collections.Generic;
using HanSocket.VO.InGame;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class PosHandler : HandlerBase
    {
        protected override string Type => "pos";

        private PosVO vo;

        protected override void OnArrived(string payload)
        {
            vo = JsonUtility.FromJson<PosVO>(payload);
        }

        protected override void OnFlag()
        {
            Debug.Log((vo.pos.x > 0 ? "오른쪽 스폰" : "왼쪽 스폰"));
        }
    }
}