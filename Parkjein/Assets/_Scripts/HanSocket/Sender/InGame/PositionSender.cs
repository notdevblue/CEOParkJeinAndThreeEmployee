using UnityEngine;
using System.Collections;
using HanSocket.VO.InGame;
using HanSocket.Data;

namespace HanSocket.Sender.InGame
{    
    public class PositionSender : MonoBehaviour
    {
        public float frame = 24.0f;

        private float _frame;
        private WaitForSecondsRealtime _wait;

        private void Start()
        {
            _frame = 1.0f / frame;
            _wait  = new WaitForSecondsRealtime(_frame);

            StartCoroutine(Send());
        }


        IEnumerator Send()
        {
            while (true)
            {
                WebSocketClient.Instance.Send(
                    "move",
                    new MoveVO(
                        UserData.Instance.myId,
                        transform.position
                        ).ToJson()
                , true);

                yield return _wait;
            }
        }
    }
}