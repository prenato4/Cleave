using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA : MonoBehaviour
{
    public float bulletSpeed = 10f; // Velocidade da bala
    public int dano = 1; // Dano causado pela bala
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Inimigo")) // Verifica se colidiu com o inimigo
        {
            Coelho coelho = collider.GetComponent<Coelho>(); // Obtém o componente Coelho do inimigo

            if (coelho != null)
            {
                coelho.ReceberDano(dano); // Chama a função ReceberDano do coelho passando a quantidade de dano
            }
        }

        Destroy(gameObject); // Destruir a bala ao colidir
    }
}