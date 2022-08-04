using System;
using HanSocket.Data;

namespace HanSocket.VO.Auth
{
   [Serializable]
   public class CheckNicknameVO : ValueObject
   {
      public string nickname;

      /// <summary>
      /// type: checknickname
      /// </summary>
      public CheckNicknameVO(string nickname)
      {
         this.nickname = nickname;
      }
   }

   [Serializable]
   public class LoginResultVO : ValueObject
   {
      public bool success;

      /// <summary>
      /// type: loginresult
      /// </summary>
      public LoginResultVO(bool success)
      {
         this.success = success;
      }
   }

   [Serializable]
   public class LoginVO : ValueObject
   {
      public string nickname;
      public string password;
      public bool signup;

      /// <summary>
      /// type: sign
      /// </summary>
      public LoginVO(string nickname, string password)
      {
         this.nickname  = nickname;
         this.password  = password;
         this.signup    = UserData.Instance.signup;
      }
   }
}