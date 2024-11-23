using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Checkpoint : MonoBehaviour
{
   public BoxCollider2D trigger;

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.CompareTag("Player"))
      {
         Rewspawn.Instance.rewSpawn = transform;
         trigger.enabled = false;
      }
   }
}
