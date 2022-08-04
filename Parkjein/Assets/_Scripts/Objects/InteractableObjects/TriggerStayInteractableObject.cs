using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.InteractableObjects
{
   public class TriggerStayInteractableObject : InteractableObjects
   {
      private void OnTriggerStay2D(Collider2D other)
      {
         OnEventTrigger(other.gameObject);
      }

      private void OnTriggerExit2D(Collider2D other)
      {
         OnEventExit(other.gameObject);
      }
   }
}