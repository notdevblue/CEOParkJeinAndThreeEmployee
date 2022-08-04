using UnityEngine;

namespace HanSocket.Handlers.Sender
{   
   public class JoinSender : MonoBehaviour
   {
      private void Start()
      {
         WebSocketClient.Instance.Send("join", "");
      }
   }
}