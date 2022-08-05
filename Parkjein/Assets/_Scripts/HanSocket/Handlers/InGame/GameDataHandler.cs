using HanSocket.Data;
using HanSocket.VO.InGame;
using UnityEngine;
using System.Collections.Concurrent;

namespace HanSocket.Handlers.InGame
{
    public class GameDataHandler : HandlerBase
    {
        protected override string Type => "gamedata";

        private GameObject _playerPrefab;

        private ConcurrentQueue<GameDataVO> vos
            = new ConcurrentQueue<GameDataVO>();


        private void Start()
        {
            _playerPrefab = Resources.Load<GameObject>("Player");
        }

        protected override void OnArrived(string payload)
        {
            vos.Enqueue(JsonUtility.FromJson<GameDataVO>(payload));
        }

        private void Update()
        {
            while (vos.Count > 0)
            {
                if (vos.TryDequeue(out var vo))
                {
                    UserData.Instance.Init(vo, _playerPrefab);
                }
            }
        }

        protected override void OnFlag()
        {
        }
    }
}