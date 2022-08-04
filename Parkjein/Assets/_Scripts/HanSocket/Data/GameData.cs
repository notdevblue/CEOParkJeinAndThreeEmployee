using System.Collections.Generic;

namespace HanSocket.Data
{
   public class GameData : Singleton<GameData>
   {
      private Dictionary<int, User> _users;

      public GameData()
      {
         _users = new Dictionary<int, User>();
      }


      public void AddUser(User user)
      {
         if (_users.ContainsKey(user.id))
            UnityEngine.Debug.LogError($"User {user.id} already added.");
         else
            _users.Add(user.id, user);
      }

      public User GetUser(int id)
      {
         if (!_users.ContainsKey(id))
         {
            UnityEngine.Debug.LogError($"Cannot find user. id:{id}");
            return null;
         }

         return _users[id];
      }

      public void DeleteUser(int id)
      {
         if (_users.ContainsKey(id))
         {
            User usr = GetUser(id);
            usr.Reset();
            usr.gameObject.SetActive(false);
            _users.Remove(id);
         }
         else 
            UnityEngine.Debug.LogError($"Cannot find user. id:{id}");
      }

      public int GetUserCount()
         => _users.Count;

      public void Clear()
      {
         _users = new Dictionary<int, User>();
      }

      public List<User> GetAllUser()
      {
         List<User> users = new List<User>();

         foreach(var item in _users)
         {
            users.Add(item.Value);
         }

         return users;
      }
   }
}