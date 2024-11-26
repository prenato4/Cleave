using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variáveis públicas para ajustar no Unity
    public float speed = 5f; // Velocidade de movimento do jogador
    public float jumpForce = 10f; // Força do pulo
    public int maxJumps = 2; // Número máximo de pulos
    public GameObject attackObject; // Objeto de ataque (defina isso no Unity)
    public float attackDuration = 0.5f; // Duração do ataque (tempo que o objeto ficará ativado)
    public GameObject bulletPrefab; // Prefab da bala (defina isso no Unity)
    public Transform firePoint; // Ponto de disparo (posicione no jogador no Unity)
    public int maxhealth;
    
    private float lastAttackTime = 0f; // Armazena o tempo do último ataque
    public float attackCooldown = 0.5f; // Tempo de espera entre ataques

    
    private bool CanDash = true;
    private bool isdashing;
    private float dashTimer = 0.2f;
    private float dashCooldown = 1f;
    public float dashCoolpower = 8f;
    
    public float gravityScale = 1f; // Gravidade personalizada do player
    private bool Jumping;
    
    private bool attckon;

    public Rigidbody2D rb; // Referência ao Rigidbody do jogador
    private Animator anim; // Referência ao Animator para controle de animações
    private int jumpsRemaining; // Número de pulos restantes
    private int currentLife; // Vida atual do jogador
    
    public float shootCooldown = 0.5f; // Tempo de espera entre os disparos
    private float lastShootTime; // Tempo do último disparo
    
    
     // Flag para controlar se a tecla Shift está pressionada
    private bool facingRight = true; // Define se o jogador está virado para a direita
    
    
    private bool controlsEnabled = true;
    
    public static int health = 100; // Vida do jogador, valor padrão

    [SerializeField] private TrailRenderer tr;

    public event Action<int> OnLifeChanged; // Evento para notificar mudanças na vida do jogador

    void Start()
    {
        // Inicializa referências e configurações iniciais
        anim = GetComponent<Animator>(); // Obtém o componente Animator
        rb = GetComponent<Rigidbody2D>(); // Obtém o componente Rigidbody2D
        rb.gravityScale = gravityScale;
        jumpsRemaining = maxJumps; // Inicializa o número de pulos restantes
        currentLife = maxhealth; // Inicializa a vida atual
        NotifyLifeChanged(); // Notifica sobre a mudança na vida
        
        attackObject.SetActive(false); // Garante que o objeto de ataque esteja desativado inicialmente
    }

    void Update()
    {
        if (isdashing)
        {
            return;
        }
        if (!controlsEnabled)
            return;
        // Atualiza o estado do jogador a cada frame
        HandleInput(); // Lida com a entrada do jogador
        Move(); // Move o jogador
        Jump(); // Faz o jogador pular
        HandleDash(); // Lida com o dash
        anim.SetFloat("YSpeed", rb.velocity.y);
        
        if (health <= 0)
        {
            Die(); // Chama a função Die se a vida for menor ou igual a zero
        }

        
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && CanDash) // Verifica se a tecla Shift está pressionada e se o cooldown foi atingido
        {
            
            StartCoroutine(Dash()); // Inicia o dash
            // lastDashTime = Time.time; // Atualiza o momento do último dash
        }
        
        
    }
    
    
    public void Curar(float quantidade)
    {
        // Cura o jogador, mas não ultrapassa a vida máxima
        health = (int)Mathf.Min(health + quantidade, maxhealth);

        // Chama o evento para notificar a mudança de vida
        currentLife = health; // Atualiza a vida atual
        NotifyLifeChanged(); // Notifica sobre a mudança na vida

        // Você pode adicionar animações ou efeitos sonoros aqui, se necessário
    }

    void HandleInput()
    {
        // Lida com a entrada do jogador para ações específicas
        // Verifica se a tecla "V" foi pressionada para iniciar o ataque
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(Attack()); // Inicia o ataque
        }
        
        // Verifica se a tecla "C" foi pressionada para disparar a bala
        if (Input.GetKeyDown(KeyCode.C) && Time.time >= lastShootTime + shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time; // Atualiza o tempo do último disparo
        }
        
        /*// Agachar
        if (Input.GetKey(KeyCode.DownArrow)) // Verifica se a tecla de seta para baixo está sendo pressionada
        {
            GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x, 0.5f); // Reduz a altura do collider para simular o agachamento
        }
        else
        {
            GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x, 0.4830437f); // Retorna a altura do collider ao normal
        }*/
        
        
    }

    void Move()
    {
        // Movimenta o jogador horizontalmente com base na entrada
        float moveInput = Input.GetAxis("Horizontal"); // Obtém a entrada horizontal (teclas de seta ou A/D)

        if (!attckon)
        {
            
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y); // Aplica a velocidade horizontal ao Rigidbody
    
            // Verifica se o jogador está se movendo e ajusta a animação apenas se não estiver pulando
            if (Jumping)
            {
                anim.SetInteger("Transition", 3); // Manter a animação de pulo enquanto estiver no ar
            }
            else if (moveInput != 0)
            {
                anim.SetInteger("Transition", 2); // Animação de correr
            }
            else
            {
                anim.SetInteger("Transition", 1); // Animação de idle
            }
            
            
            
        
        }
       
        

        /// Verifica a direção do movimento e faz o jogador virar se necessário
        if (moveInput > 0 && facingRight && !attckon)
        {
            Flip(); // Vira o jogador para a direita
        }
        else if (moveInput < 0 && !facingRight && !attckon)
        {
            Flip(); // Vira o jogador para a esquerda
        }
    }
    
    // Método para alterar a gravidade do player
    public void SetGravity(float newGravityScale)
    {
        rb.gravityScale = newGravityScale;
    }

    void Jump()
    {
        // Faz o jogador pular se as condições forem atendidas
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0 && !isdashing) // Verifica se a tecla de espaço foi pressionada, ainda há pulos restantes e não está realizando um dash
        {
            anim.SetInteger("Transition", 3); // Animação de pulo
            Jumping = true;
            AudioObserver.OnPlaySfxEvent("Jump");
            ParticleObserver.onParticleSpawn(transform.position);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Aplica a força de pulo ao Rigidbody
            jumpsRemaining--; // Decrementa o número de pulos restantes

            //StartCoroutine(ReturnToIdle()); // Retorna para a animação de idle após um tempo
            
            
        }
    }

    void ReturnToIdle()
    {
        // Espera um tempo e então volta para a animação de idle
        //yield return new WaitForSeconds(0.8f); // Ajustado para voltar mais rapidamente
        Jumping = false; // Atualiza o estado de pulo
        anim.SetInteger("Transition", 1); // Volta para a animação de idle
    }

    void HandleDash()
    {
        // Lida com a execução do dash se as condições forem atendidas
        //if (Input.GetKeyDown(KeyCode.LeftShift) && CanDash) // Verifica se a tecla Shift está pressionada e se o cooldown foi atingido
        {
            
               //StartCoroutine(Dash()); // Inicia o dash
               // lastDashTime = Time.time; // Atualiza o momento do último dash
        }
    }

    IEnumerator Dash()
    {
        CanDash = false;
        isdashing = true;
        anim.SetInteger("Transition", 15);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2((facingRight ? -1 : 1) * dashCoolpower, 0f);

        tr.emitting = true;
        yield return new WaitForSeconds(dashTimer);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isdashing = false;
        yield return new WaitForSeconds(dashCooldown);
        CanDash = true;
        
        
    }

    void Shoot()
    {   
        
        anim.SetTrigger("shot");
       
        // Cria uma bala no ponto de disparo e define sua direção
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); // Instancia a bala

        SA bulletScript = bullet.GetComponent<SA>(); // Obtém o script da bala

        // Define a direção da bala dependendo de para onde o jogador está virado
        if (facingRight)
        {
            bulletScript.SetDirection(Vector2.left); // Atira para a direita
        }
        else
        {
            bulletScript.SetDirection(Vector2.right); // Atira para a esquerda
        }
    }

    public void Damage(int DM)
    {
        // Aplica dano ao jogador e atualiza a vida
        health -= DM;
        anim.SetTrigger("Hit");
        if (health < 0) health = 0; // Garantir que a saúde não seja negativa
        currentLife = health; // Atualiza currentLife para refletir a nova saúde
        NotifyLifeChanged(); // Notifica sobre a mudança na vida
    }
    
    public void DisableControls() 
    {
        controlsEnabled = false;
        rb.velocity = Vector2.zero; // Para o movimento imediatamente
       
    }

    public void EnableControls()
    {
        controlsEnabled = true;
       
    }

    public void Heal(int amount)
    {
        // Cura o jogador e atualiza a vida
        health += amount;
        if (health > maxhealth) health = maxhealth; // Garantir que a saúde não ultrapasse o máximo
        currentLife = health; // Atualiza currentLife para refletir a nova saúde
        NotifyLifeChanged(); // Notifica sobre a mudança na vida
    }

    private void NotifyLifeChanged()
    {
        // Notifica sobre a mudança na vida do jogador
        OnLifeChanged?.Invoke(currentLife);
    }

    void Die()
    {
        // Ação a ser realizada quando o jogador morrer
        //Debug.Log("Player morreu!");
        anim.SetInteger("Transition", 16);
    }

    void Flip()
    {
        // Vira o jogador para a direção oposta
        facingRight = !facingRight; // Inverte a direção

        // Multiplica a escala em X por -1 para virar o jogador visualmente
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    IEnumerator Attack()
    {
        // Verifica se o cooldown de ataque passou
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time; // Atualiza o tempo do último ataque

            anim.SetInteger("Transition", 4); // Define a animação de ataque
            attckon = true;

            // Aguarda 0.2 segundos antes de ativar o objeto de ataque
            yield return new WaitForSeconds(0.2f);

            attackObject.SetActive(true); // Ativa o objeto de ataque

            // Aguarda pelo tempo de duração do ataque
            yield return new WaitForSeconds(attackDuration);

            attackObject.SetActive(false); // Desativa o objeto de ataque
            anim.SetInteger("Transition", 1); // Volta para a animação de idle
            attckon = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    { // Verifica se o jogador colidiu com o chão
        if (other.gameObject.layer == 6) // Checa se a colisão foi com o chão
        {
            //anim.SetInteger("Transition", 1); // Define a animação para idle
            Jumping = false;
            jumpsRemaining = maxJumps; // Reinicia o número de pulos restantes ao tocar no chão
        }
        
    }
}