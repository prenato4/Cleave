using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respanwfase : MonoBehaviour
{
    public static respanwfase Instance;
    public Transform rewSpawn;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = rewSpawn.position;
        }
    }
}
