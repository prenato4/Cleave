using System;
using System.Collections;
using UnityEngine;

public class Clotho : MonoBehaviour
{
    public AudioClip shieldSound; // Som do escudo
    private AudioSource audioSource; // Componente de áudio
    public GameObject objectToActivate;
    public Transform player; // Referência ao player
    public GameObject projectilePrefab; // Prefab do projétil
    public Transform firePoint; // Ponto de disparo do projétil
    public float moveSpeed = 2f; // Velocidade de movimento do boss
    public float stopRange = 2f; // Distância mínima para o boss parar de se mover
    public float attackCooldown = 10f; // Tempo entre os ataques
    public float attackStartDelay = 10f; // Delay antes de iniciar o ataque
    public GameObject shield; // Referência ao escudo de Clotho

    private Animator animator; // Referência ao componente Animator
    private bool canAttack = true; // Controle de cooldown para ataque
    private bool isShieldActive = false; // Estado de ativação do escudo
    private bool isInvulnerable = false; // Controla a invulnerabilidade de Clotho

    public int maxHealth = 100; // Vida máxima de Clotho
    public int currentHealth; // Vida atual de Clotho

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Inicializa a vida de Clotho
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        StartCoroutine(StartWithDelays());
        StartCoroutine(ActivateShieldPeriodically()); // Inicia a ativação do escudo a cada 15 segundos
    }

    IEnumerator StartWithDelays()
    {
        // Aguarda o tempo de delay antes de iniciar o ataque
        yield return new WaitForSeconds(attackStartDelay);

        while (true)
        {
            if (Vector2.Distance(transform.position, player.position) <= stopRange && canAttack)
            {
                StartCoroutine(Attack());
            }

            // Aguarda um pequeno intervalo para verificar novamente (ajuste conforme necessário)
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator ActivateShieldPeriodically()
    {
        // Aguarda antes de ativar o escudo pela primeira vez
        yield return new WaitForSeconds(16f); // Delay inicial antes do primeiro escudo

        while (true)
        {
            // Ativa o escudo
            ActivateShield();

            // Aguarda o tempo que o escudo ficará ativo
            yield return new WaitForSeconds(5f); // O escudo ficará ativo por 5 segundos

            // Desativa o escudo
            DeactivateShield();

            // Aguarda 10 segundos antes de ativar novamente
            yield return new WaitForSeconds(15f);
        }
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        // Inverte a direção do sprite para olhar para o player
        Vector3 scale = transform.localScale;
        scale.x = player.position.x < transform.position.x ? 1 : -1;
        transform.localScale = scale;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopRange)
        {
            // Move em direção ao player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Ajusta a animação para "Andar"
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Para de se mover
            animator.SetBool("isWalking", false);
        }
    }

    IEnumerator Attack()
    {
        
        canAttack = false;

        // Inicia a animação de ataque
        animator.SetTrigger("isAttacking");

        // Aguarda o tempo suficiente para a animação de ataque ser exibida (ajuste o valor conforme necessário)
        yield return new WaitForSeconds(0.5f);

        // Instancia o projétil
        if (firePoint != null && projectilePrefab != null)
        {
            audioSource.PlayOneShot(shieldSound);
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            tiroclotho projectileScript = projectile.GetComponent<tiroclotho>();

            // Define a direção do projétil
            Vector2 direction = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
            projectileScript.SetDirection(direction);
        }

        // Aguarda o cooldown do ataque
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void ActivateShield()
    {
        // Ativa o escudo e torna Clotho invulnerável
        if (shield != null)
        {
            shield.SetActive(true); // Ativa o escudo
            isInvulnerable = true; // Clotho fica invulnerável
            
        }
    }

    void DeactivateShield()
    {
        // Desativa o escudo e permite que Clotho tome dano novamente
        if (shield != null)
        {
            shield.SetActive(false); // Desativa o escudo
            isInvulnerable = false; // Clotho não está mais invulnerável
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInvulnerable) return; // Ignora o dano se Clotho estiver invulnerável
    }

    // Método para aplicar dano
    public void Damage(int damage)
    {
        if (isInvulnerable) return; // Ignora dano se Clotho estiver invulnerável

        animator.SetTrigger("hit");

        // Reduz a vida
        currentHealth -= damage;

        // Garantir que a vida não seja menor que 0
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Exibir a vida restante no console (pode ser substituído por uma UI de vida)
        Debug.Log("Vida de Clotho: " + currentHealth);

        // Verifica se Clotho morreu
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método para lidar com a morte de Clotho
    void Die()
    {
        // Lógica para a morte de Clotho (exemplo: desativa o objeto ou inicia uma animação)
        Debug.Log("Clotho morreu!");
        animator.SetTrigger("death");
        Destroy(gameObject, 1.5f); // Exemplo de desativar o objeto (Clotho morreu)
        objectToActivate.SetActive(true);
    }
}
