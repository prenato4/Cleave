using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variáveis públicas para ajustar no Unity
    public float speed = 5f; // Velocidade de movimento do jogador
    public float jumpForce = 10f; // Força do pulo
    public int maxJumps = 2; // Número máximo de pulos

    private Rigidbody2D rb; // Referência ao Rigidbody do jogador
    private int jumpsRemaining; // Número de pulos restantes

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D do jogador no início
        jumpsRemaining = maxJumps; // Inicializa o número de pulos restantes
    }

    void Update()
    {
        // Movimento horizontal
        float moveInput = Input.GetAxis("Horizontal"); // Obtém a entrada horizontal (teclas de seta)
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y); // Aplica a velocidade horizontal ao Rigidbody

        // Pular
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0) // Verifica se a tecla de espaço foi pressionada e ainda há pulos restantes
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Aplica a força de pulo ao Rigidbody
            jumpsRemaining--; // Decrementa o número de pulos restantes
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) // Verifica se colidiu com o chão
        {
            jumpsRemaining = maxJumps; // Reinicia o número de pulos restantes ao tocar no chão
        }
    }
}