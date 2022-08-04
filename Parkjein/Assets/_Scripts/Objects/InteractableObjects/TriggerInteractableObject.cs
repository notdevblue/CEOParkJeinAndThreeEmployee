using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.InteractableObjects
{
   public class TriggerInteractableObject : InteractableObjects
   {
      #region Unity Trigger Event
      private void OnTriggerEnter2D(Collider2D other)
      {
         OnEventTrigger(other.gameObject);
      }

      private void OnTriggerExit2D(Collider2D other)
      {
         if (EventIsToggle) return;

         OnEventExit(other.gameObject);
      }
      #endregion // Unity Trigger Event
   }
}