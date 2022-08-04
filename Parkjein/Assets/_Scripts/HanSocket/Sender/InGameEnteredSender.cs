using HanSocket.Data;
using UnityEngine;

namespace HanSocket.Sender
{   
   public class InGameEnteredSender : MonoBehaviour
   {
      private void Awake()
      {
         GameData.Instance.Clear();

         WebSocketClient.Instance.Send("loaded", "");
      }
   }
}