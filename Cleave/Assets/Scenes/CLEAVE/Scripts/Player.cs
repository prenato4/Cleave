using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variáveis públicas para ajustar no Unity
    public float speed = 5f; // Velocidade de movimento do jogador
    public float jumpForce = 10f; // Força do pulo
    public int maxJumps = 2; // Número máximo de 

    public float dashDuration;
    
    public float dashForce = 5f; // Força do dash
    private bool isDashing = false;
    bool isShiftPressed = false; // Flag para controlar se a tecla Shift está pressionada
    
    public float dashCooldown = 15f; // Cooldown do dash em segundos
    private float lastDashTime; // Momento do último dash

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
    
        if (!isDashing)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y); // Aplica a velocidade horizontal ao Rigidbody
        }

        // Pular
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0 && !isDashing) // Verifica se a tecla de espaço foi pressionada, ainda há pulos restantes e não está realizando um dash
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Aplica a força de pulo ao Rigidbody
            jumpsRemaining--; // Decrementa o número de pulos restantes
        }
    
        // Agachar
        if (Input.GetKey(KeyCode.DownArrow)) // Verifica se a tecla de seta para baixo está sendo pressionada
        {
            // Reduz a altura do collider para simular o agachamento
            GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x, 0.5f); 
        }
        else
        {
            // Retorna a altura do collider ao normal
            GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x, 1f); 
        }

        // Verificar se a tecla Shift está sendo pressionada
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isShiftPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isShiftPressed = false;
        }

        // Dash
        if (isShiftPressed && Time.time - lastDashTime > dashCooldown) // Verifica se a tecla Shift está pressionada e se o cooldown foi atingido
        {
            float dashInputHorizontal = Input.GetAxis("Horizontal");
            float dashInputVertical = Input.GetAxis("Vertical");

            if (dashInputHorizontal != 0 || dashInputVertical != 0)
            {
                StartCoroutine(Dash(dashInputHorizontal, dashInputVertical));
                lastDashTime = Time.time; // Atualiza o momento do último dash
            }
        }
    }

    IEnumerator Dash(float horizontalInput, float verticalInput)
    {
        isDashing = true; // Define que o jogador está realizando um dash

        Vector2 dashDirection = new Vector2(horizontalInput, verticalInput).normalized; // Obtém a direção do dash normalizada
        rb.velocity = dashDirection * dashForce; // Aplica a força do dash na direção especificada
    
        yield return new WaitForSeconds(0.5f); // Tempo de duração do dash
        isDashing = false; // Finaliza o dash
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) // Verifica se colidiu com o chão
        {
            jumpsRemaining = maxJumps; // Reinicia o número de pulos restantes ao tocar no chão
        }
    }
}