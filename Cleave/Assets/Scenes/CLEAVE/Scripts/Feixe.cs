using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feixe : MonoBehaviour
{
    public float projectileSpeed = 10f; // Velocidade do projétil
    private Rigidbody2D rb;
    private Vector2 direction;

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Define a velocidade do projétil com base na direção fornecida
            rb.velocity = direction * projectileSpeed;
        }
    }

    private void Update()
    {
        // Destroi o projétil após 1 segundo
        Destroy(gameObject, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}