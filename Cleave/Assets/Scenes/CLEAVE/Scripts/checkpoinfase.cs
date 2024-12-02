using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoinfase : MonoBehaviour
{
    public BoxCollider2D trigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            respanwfase.Instance.rewSpawn = transform;
            trigger.enabled = false;
        }
    }
}
