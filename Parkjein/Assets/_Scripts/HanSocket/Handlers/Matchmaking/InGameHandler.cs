using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HanSocket.Handlers.Matchmaking
{
    public class InGameHandler : HandlerBase
    {
        protected override string Type => "ingame";

        protected override void OnArrived(string payload)
        {
        }

        protected override void OnFlag()
        {
            Debug.Log("메치메이킹 완료");
            SceneManager.LoadScene("InGameScene");
        }
    }
}