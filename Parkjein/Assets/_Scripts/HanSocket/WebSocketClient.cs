using System.Collections;
using System.Collections.Generic;
using HanSocket.VO;
using UnityEngine;
using WebSocketSharp;

namespace HanSocket
{
   public class WebSocketClient : MonoSingleton<WebSocketClient>
   {
      [Header("Server Address")]
      public string ipAddr;

      [Header("Server Port")]
      public string port;

      private WebSocket ws;

      protected override void Awake()
      {
         base.Awake();
         DontDestroyOnLoad(this.gameObject);
      }


      public void Connect(string ipAddr, string port)
      {
         this.ipAddr = ipAddr;
         this.port   = port;

         ws = new WebSocket($"ws://{ipAddr}:{port}");
         Debug.Log(ipAddr);
         Debug.Log(port);

         ws.OnMessage += (sender, e) => {
            Debug.Log($"Arrived: {e.Data}");

            DataVO data = JsonUtility.FromJson<DataVO>(e.Data);
            BufferHandler.Instance.Handle(data.type, data.payload);
         };

         ws.Connect();
      }

      public void Disconnect(CloseStatusCode code = CloseStatusCode.Normal,
                             string reason = null)
      {
         ws.Close(code, ((reason == null) ? "Client disconnected" : reason));
      }

      public void Send(string type, string payload, bool nolog = false)
      {
         if(!nolog)
            Debug.Log($"Sending type:{type} payload:{payload}");
         ws.Send(JsonUtility.ToJson(new DataVO(type, payload)));
      }

      private void OnApplicationQuit()
      {
         Disconnect();
      }


   }
}