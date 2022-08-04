using HanSocket.Data;
using HanSocket.VO.InGame;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class GameDataHandler : HandlerBase
    {
        protected override string Type => "gamedata";

        private GameObject _playerPrefab;
        private GameDataVO vo;

        private void Start()
        {
            _playerPrefab = Resources.Load<GameObject>("Player");
        }

        protected override void OnArrived(string payload)
        {
            vo = JsonUtility.FromJson<GameDataVO>(payload);
        }

        protected override void OnFlag()
        {
            UserData.Instance.Init(vo, _playerPrefab);
        }
    }
}