using System;
using System.Collections.Generic;
using UnityEngine;

namespace HanSocket.VO.Player
{
   [Serializable]
   public class BulletDataVO : ValueObject
   {
      public List<Bullet> bullets;
   }

   [Serializable]
   public class Bullet
   {
      public int id;
      public Vector2 pos;

      public Bullet(int id, Vector2 pos)
      {
         this.id = id;
         this.pos = pos;
      }
   }
}