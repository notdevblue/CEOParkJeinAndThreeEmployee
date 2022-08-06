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

        private PosVO vo;

        protected override void OnArrived(string payload)
        {
            vo = JsonUtility.FromJson<PosVO>(payload);
        }

        protected override void OnFlag()
        {
            List<PlayerUI> uis = MonoBehaviour.FindObjectsOfType<PlayerUI>().ToList();
            Debug.Log((vo.pos.x > 0 ? "오른쪽 스폰" : "왼쪽 스폰"));
            string myName = (vo.pos.x > 0 ? "Right" : "Left");
            string otherName = (vo.pos.x > 0 ? "Left" : "Right");


            foreach (int key in UserData.Instance.users.Keys)
            {
                UserData.Instance.users[key].GetComponent<PlayerData>().MyUI = 
                    uis.Find(x => x.gameObject.name == (key.Equals(WebSocketClient.Instance.id) ? myName : otherName));
            }
        }
    }
}