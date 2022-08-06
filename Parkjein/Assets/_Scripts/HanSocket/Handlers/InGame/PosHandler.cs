using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HanSocket.Data;
using HanSocket.VO.InGame;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class PosHandler : HandlerBase
    {
        protected override string Type => "pos";

        private PosVO _vo;

        public PlayerUI left;
        public PlayerUI right;


        protected override void OnArrived(string payload)
        {
            _vo = JsonUtility.FromJson<PosVO>(payload);
        }

        protected override void OnFlag()
        {
            StartCoroutine(InitUI());
        }

        IEnumerator InitUI()
        {
            PosVO vo = _vo;
            yield return new WaitUntil(() => UserData.Instance.users.Count >= 2);

            PlayerUI me = vo.pos.x > 0 ? right : left;
            PlayerUI you = vo.pos.x > 0 ? left : right;

            foreach (int key in UserData.Instance.users.Keys)
            {
                UserData.Instance.users[key]
                    .GetComponent<PlayerData>().MyUI =
                        (key == WebSocketClient.Instance.id ? me : you);
            }
        }
    }
}