using System.Collections;
using System.Collections.Concurrent;
using HanSocket.Data;
using HanSocket.VO.InGame;
using UI.InGame;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class GameStartHandler : HandlerBase
    {
        protected override string Type => "gamestart";
        private ConcurrentQueue<RespawnVO> vos
            = new ConcurrentQueue<RespawnVO>();

        public GameObject cvsSkill;
        public MainPanel mainPanel;


        protected override void OnArrived(string payload)
        {
            vos.Enqueue(JsonUtility.FromJson<RespawnVO>(payload));
        }

        protected override void OnFlag()
        {
            cvsSkill.SetActive(false);
            mainPanel.Open();
        }

        private void Update()
        {
            while (vos.Count > 0)
            {
                if (vos.TryDequeue(out var vo))
                {
                    GameObject user   = UserData.Instance.users[vo.id];
                    Rigidbody2D rigid = user.GetComponent<Rigidbody2D>();

                    if(rigid != null)
                        rigid.velocity = Vector2.zero;

                    user.GetComponent<Remote>()
                        ?.SetTarget(vo.pos);

                    user.transform.position = vo.pos;
                    user.SetActive(true);
                }
            }
        }
    }
}