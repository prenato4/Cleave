using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hunt : MonoBehaviour
{
    public GameObject arrowPrefab;  // Prefab da flecha
    public GameObject trapPrefab;  // Prefab da armadilha
    public Transform firePoint;    // Ponto de onde a flecha será disparada
    public float fireInterval = 10f; // Intervalo de tempo entre disparos
    public float animationResetDelay = 1f; // Tempo após o disparo para voltar à animação padrão
    public float trapMinDistance = 2f; // Distância mínima onde a armadilha pode ser colocada
    public float trapMaxDistance = 5f; // Distância máxima onde a armadilha pode ser colocada
    public float attackRange = 5f; // Distância mínima para atacar o player
    public float moveSpeed = 2f;   // Velocidade de movimento do boss

    public int maxHealth = 100;  // Vida máxima do boss
    public int currentHealth;    // Vida atual do boss

    public float spawnInterval = 15f;

    private bool facingRight = true; // Define se o boss está virado para a direita
    private Animator anim;
    private Transform player;
    private bool isFiring;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        // Encontrar o player na cena
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Iniciar a rotina de disparo
        StartCoroutine(FireArrow());

        // Iniciar a rotina de armadilhas
        StartCoroutine(spawnTrapp());
    }

    private void Update()
    {
        if (!isFiring)
        {
            anim.SetInteger("Transition", 1); // Idle
        }

        // Atualiza a direção para onde o boss está olhando com base na posição do player
        FlipTowardsPlayer();

        // Movimenta o boss até estar dentro do alcance para atacar
        MoveTowardsPlayerIfNeeded();
    }

    void FlipTowardsPlayer()
    {
        // Verifica se o player está à direita ou à esquerda e ajusta o boss se necessário
        if (player.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        // Inverte o estado de `facingRight`
        facingRight = !facingRight;

        // Altera o eixo X do `localScale` para inverter o sprite
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * (facingRight ? -1 : 1); // Define positivo para direita, negativo para esquerda
        transform.localScale = localScale;
    }

    void MoveTowardsPlayerIfNeeded()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Se estiver fora do alcance de ataque, move-se em direção ao player
        if (distanceToPlayer > attackRange)
        {
            anim.SetInteger("Transition", 3); // Animação de andar
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetInteger("Transition", 1); // Idle ao chegar no range
        }
    }

    IEnumerator FireArrow()
    {
        while (true)
        {
            // Espera o intervalo de disparo
            yield return new WaitForSeconds(fireInterval);

            // Verifica a distância antes de disparar
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                ShootAtPlayer();
            }
        }
    }

    IEnumerator spawnTrapp()
    {
        while (true)
        {
            // Espera o intervalo de spawn
            yield return new WaitForSeconds(spawnInterval);
            StartCoroutine(SpawnTrap());
        }
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;  // Reduz a vida

        if (currentHealth > 0)
        {
            anim.SetTrigger("hit");
            Debug.Log("Boss recebeu dano! Vida atual: " + currentHealth);
        }
        else
        {
            Die();
        }
    }

    void ShootAtPlayer()
    {
        isFiring = true;

        // Troca para a animação de ataque
        anim.SetInteger("Transition", 2);

        // Cria uma flecha no ponto de disparo
        GameObject bullet = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

        SA bulletScript = bullet.GetComponent<SA>();

        // Define a direção da flecha
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        bulletScript.SetDirection(direction);

        // Retorna à animação padrão após um pequeno delay
        StartCoroutine(ResetToDefaultAnimation());
    }

    private IEnumerator SpawnTrap()
    {
        yield return new WaitForSeconds(10f);
        anim.SetTrigger("sp");

        // Gera uma distância aleatória dentro do intervalo definido
        float randomDistance = Random.Range(trapMinDistance, trapMaxDistance);

        // Calcula a posição da armadilha
        Vector2 spawnDirection = facingRight ? Vector2.right : Vector2.left;
        Vector2 spawnPosition = (Vector2)transform.position + spawnDirection * randomDistance;

        // Cria a armadilha na posição calculada
        Instantiate(trapPrefab, spawnPosition, Quaternion.identity);
    }

    IEnumerator ResetToDefaultAnimation()
    {
        yield return new WaitForSeconds(animationResetDelay);
        anim.SetInteger("Transition", 1); // Idle
        isFiring = false;
    }

    void Die()
    {
        anim.SetTrigger("death");
        Destroy(gameObject, 0.5f);
    }
}
