using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiroclotho : MonoBehaviour
{
    public float bulletSpeed = 10f; // Velocidade da bala
    public int attackDamage = 1; // Dano causado pela bala
    private Rigidbody2D rb;
    private Vector2 bulletDirection; // Direção da bala

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = bulletDirection * bulletSpeed; // Aplica a velocidade da bala na direção passada
    }

    // Método para definir a direção da bala
    public void SetDirection(Vector2 direction)
    {
        bulletDirection = direction.normalized; // Normaliza a direção para evitar variações na velocidade
    }

    private void Update()
    {
        Destroy(gameObject, 2f); // Destruir a bala após 2 segundos
    }

    // Usando OnTriggerEnter2D em vez de OnCollisionEnter2D
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se a bala colidiu com o player
        if (other.CompareTag("Player"))
        {
            // Aplica dano ao player
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage(attackDamage);
            }

            // Destrói a bala após o impacto
            Destroy(gameObject);
        }
        else
        {
            // Destrói a bala se bater em qualquer outra coisa
            Destroy(gameObject);
        }
    }
}
