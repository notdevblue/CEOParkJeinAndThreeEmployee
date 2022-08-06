using System.Collections;
using System.Collections.Concurrent;
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

        public int id;

        private TMPro.TMP_Text text;
        public TMPro.TMP_Text Text
        {
            get
            {
                if (text == null)
                {
                    text = GameObject.Find("LogCanvas").GetComponentInChildren<TMPro.TMP_Text>();
                }
                return text;
            }
        }


        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            Connect(ipAddr, port);
        }


        public void Connect(string ipAddr, string port)
        {
            this.ipAddr = ipAddr;
            this.port = port;

            ws = new WebSocket($"ws://{ipAddr}:{port}");

            ws.OnMessage += (sender, e) =>
            {

                DataVO data = JsonUtility.FromJson<DataVO>(e.Data);
                BufferHandler.Instance.Handle(data.type, data.payload);

                if (data.type != "move" && data.type != "bulletstop")
                {
                    Debug.Log($"Arrived: {e.Data}");
                    q.Enqueue(e.Data.ToString());
                }

            };

            ws.Connect();
        }

        ConcurrentQueue<string> q = new ConcurrentQueue<string>();

        private void FixedUpdate()
        {
            if (q.Count > 0 && q.TryDequeue(out string rawpacket))
                Text.text = (rawpacket.ToString() + "\n") + Text.text;
        }

        public void Disconnect(CloseStatusCode code = CloseStatusCode.Normal,
                               string reason = null)
        {
            ws.Close(code, ((reason == null) ? "Client disconnected" : reason));
        }

        public void Send(string type, string payload, bool nolog = false)
        {
            if (!nolog)
                Debug.Log($"Sending type:{type} payload:{payload}");
            ws.Send(JsonUtility.ToJson(new DataVO(type, payload)));
        }

        private void OnApplicationQuit()
        {
            Disconnect();
        }


    }
}