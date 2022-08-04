using HanSocket.Data;
using HanSocket.VO.InGame;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class GameStartHandler : HandlerBase
    {
        protected override string Type => "gamestart";

        GameStartVO vo;

        protected override void OnArrived(string payload)
        {
            vo = JsonUtility.FromJson<GameStartVO>(payload);
        }

        protected override void OnFlag()
        {
            UserData.Instance.Init(vo);
        }
    }
}