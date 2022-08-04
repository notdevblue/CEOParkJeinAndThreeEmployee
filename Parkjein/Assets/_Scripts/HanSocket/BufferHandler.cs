using UnityEngine;
using System;
using System.Collections.Generic;

namespace HanSocket
{
   public class BufferHandler : Singleton<BufferHandler>
   {
      private Dictionary<string, Action<string>> _handlerDictionary;

      public BufferHandler()
      {
         _handlerDictionary = new Dictionary<string, Action<string>>();
      }

      public void Handle(string type, string payload)
      {
         if (!_handlerDictionary.ContainsKey(type))
         {
            Debug.LogError($"Cannot find handler for {type}");
            return;
         }

         _handlerDictionary[type](payload);
      }

      /// <summary>
      /// type 에 대한 callback 을 추가합니다.
      /// </summary>
      /// <param name="type">Packet type</param>
      /// <param name="callback">type 에 대한 callback</param>
      public void AddHandler(string type, Action<string> callback)
      {
         if(_handlerDictionary.ContainsKey(type))
         {
            Debug.LogWarning($"Type: {type} is already added, ignoring request.");
            return;
         }
         _handlerDictionary.Add(type, callback);
      }
   }
}