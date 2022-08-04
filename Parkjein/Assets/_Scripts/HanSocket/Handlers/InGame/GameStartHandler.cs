using HanSocket.Data;
using HanSocket.VO.InGame;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class GameStartHandler : HandlerBase
    {
        protected override string Type => "gamestart";

        private GameObject _playerPrefab;
        private GameStartVO vo;

        private void Start()
        {
            _playerPrefab = Resources.Load<GameObject>("Player");
        }

        protected override void OnArrived(string payload)
        {
            vo = JsonUtility.FromJson<GameStartVO>(payload);
        }

        protected override void OnFlag()
        {
            UserData.Instance.Init(vo, _playerPrefab);
        }
    }
}