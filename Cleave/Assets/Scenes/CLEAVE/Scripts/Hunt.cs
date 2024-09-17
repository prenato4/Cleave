using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hunt : MonoBehaviour
{
    public GameObject arrowPrefab;  // Prefab da flecha
    public GameObject trapPrefab;     // Prefab da armadilha
    public Transform firePoint;     // Ponto de onde a flecha será disparada
    public float fireInterval = 10f; // Intervalo de tempo entre disparos
    public float animationResetDelay = 1f; // Tempo após o disparo para voltar à animação padrão
    public float trapMinDistance = 2f; // Distância mínima onde a armadilha pode ser colocada
    public float trapMaxDistance = 5f; // Distância máxima onde a armadilha pode ser colocada
    
    public int maxHealth = 100;  // Vida máxima do boss
    public int currentHealth;   // Vida atual do boss


    private bool facingRight = true; // Define se o boss está virado para a direita
    
    private Animator anim;
    private Transform player;
    private bool Fire;

    void Start()
    {
        
        currentHealth = maxHealth;
        
        anim = GetComponent<Animator>();
        // Encontrar o player na cena
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Iniciar a rotina de disparo
        StartCoroutine(FireArrow());
    }

    private void Update()
    {
        if (!Fire)
        {
            anim.SetInteger("Transition", 1);
        }

        // Atualiza a direção para onde o boss está olhando com base na posição do player
        FlipTowardsPlayer();
    }

    void FlipTowardsPlayer()
    {
        // Verifica se o player está à direita ou à esquerda
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
        // Inverte a direção do boss
        facingRight = !facingRight;

        // Multiplica o scale do transform para inverter visualmente o sprite
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    IEnumerator FireArrow()
    {
        while (true)
        {
            // Espera 10 segundos
            yield return new WaitForSeconds(fireInterval);

            // Disparar flecha na direção do player
            ShootAtPlayer();
            
            // Spawn armadilha aleatoriamente
            SpawnTrap();
        }
    }
    
     public void Damage(int damage)
        {
            currentHealth -= damage;  // Reduz a vida
    
            // Verifica se o boss ainda tem vida
            if (currentHealth > 0)
            {
                // Se quiser adicionar uma animação de hit, faça isso aqui
                Debug.Log("Boss recebeu dano! Vida atual: " + currentHealth);
            }
            else
            {
                // Se a vida chegar a zero, chama o método para morte
                Die();
            }
        }

    void ShootAtPlayer()
    {
        Fire = true;
        // Troca para a animação de ataque
        anim.SetInteger("Transition", 2);

        // Cria uma flecha no ponto de disparo e define sua direção
        GameObject bullet = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation); // Instancia a flecha

        SA bulletScript = bullet.GetComponent<SA>(); // Obtém o script da flecha

        // Define a direção da flecha dependendo de para onde o boss está virado
        if (facingRight)
        {
            bulletScript.SetDirection(Vector2.right); // Atira para a direita
        }
        else
        {
            bulletScript.SetDirection(Vector2.left); // Atira para a esquerda
        }

        // Retornar à animação padrão após um pequeno delay
        StartCoroutine(ResetToDefaultAnimation());
    }
    
    void SpawnTrap()
    {
        // Gera uma distância aleatória dentro do intervalo definido
        float randomDistance = Random.Range(trapMinDistance, trapMaxDistance);

        // Calcula a posição da armadilha com base na direção do boss
        Vector2 spawnDirection = facingRight ? Vector2.right : Vector2.left;
        Vector2 spawnPosition = (Vector2)transform.position + spawnDirection * randomDistance;

        // Cria a armadilha na posição calculada
        Instantiate(trapPrefab, spawnPosition, Quaternion.identity);
    }
    
    

    IEnumerator ResetToDefaultAnimation()
    {
        // Espera um tempo para a animação de ataque terminar
        yield return new WaitForSeconds(animationResetDelay);

        // Retorna para a animação padrão (idle, por exemplo)
        anim.SetInteger("Transition", 1);  // Aqui, 1 seria a animação padrão de idle
    }
    
     void Die()
        {
          
    
            // Desativa ou destrói o boss após a morte
             Destroy(gameObject); // Se quiser destruir o objeto
        }
}
