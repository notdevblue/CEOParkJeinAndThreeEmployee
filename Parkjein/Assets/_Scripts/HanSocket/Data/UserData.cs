using UnityEngine;

namespace HanSocket.Data
{
   public class UserData : Singleton<UserData>
   {
      public int id = -1;
      public bool signup = false;
      public string nickname = "";

      public User Player { get; set; }
   }
}