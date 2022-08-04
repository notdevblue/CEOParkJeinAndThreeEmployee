using System.Collections;
using UnityEngine;

namespace HanSocket.Handlers
{   
   abstract public class HandlerBase : MonoBehaviour
   {
      private Flag _flag = new Flag();
      private WaitUntil wait;
      abstract protected string Type { get; }

      protected virtual void Awake()
      {
         wait = new WaitUntil(_flag.Get);

         BufferHandler.Instance.AddHandler(Type, payload => {
            OnArrived(payload);
            _flag.Set();
         });

         StartCoroutine(FlagAwaiter());
      }

      private IEnumerator FlagAwaiter()
      {
         while (true)
         {
            yield return wait;
            OnFlag();
         }
      }

      /// <summary>
      /// 도착시 웹소켓 쓰레드에서 호출됨
      /// </summary>
      abstract protected void OnArrived(string payload);

      /// <summary>
      /// OnArrived 후 호출됨
      /// </summary>
      abstract protected void OnFlag();
   }
}