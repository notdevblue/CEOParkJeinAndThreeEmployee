using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.InteractableObjects
{
   public class CollisionStayInteractableObject : InteractableObjects
   {
      public Vector3 colPosition;
      public bool isOn = true;

      private GameObject _curCollision;

      private void OnCollisionStay2D(Collision2D other)
      {
         if (!isOn) return;
         colPosition = other.contacts[0].point;
         _curCollision = other.gameObject;

         OnEventTrigger(other.gameObject);
      }

      private void OnCollisionExit2D(Collision2D other)
      {
         if (!isOn || _curCollision != other.gameObject) return;

         OnEventExit(other.gameObject);
      }
   }
}