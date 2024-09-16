using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patinho : MonoBehaviour
{
    public float speed = 2f; // Velocidade de movimento do patinho
    public Vector2 movePosition; // Distância máxima que o patinho se move
    public Transform moveDestination; // Ponto final para o movimento do patinho

    private Vector2 startPosition; // Posição inicial do patinho
    private Vector2 moveTarget; // Ponto alvo para o movimento
    private Vector2 currentMoveDirection; // Direção atual do movimento
    private bool isReturning = false; // Indica se o patinho está voltando ao ponto inicial

    public int patinhoHealth = 5; // Vida do patinho
    public int patinhoDamage = 1; // Dano que o patinho causa ao jogador
    public float attackRange = 2f; // Distância mínima para o patinho atacar
    public float attackCooldown = 1.5f; // Tempo entre os ataques do patinho
    private float nextAttackTime = 0f; // Controla o tempo de espera entre ataques

    private Transform player;
    private Player playerController;
    private bool isChasing = false; // Indica se o patinho está seguindo o jogador
    public bool playerAttacked = false; // Indica se o jogador atacou o patinho

    void Start()
    {
        startPosition = transform.position; // Define a posição inicial do patinho
        player = GameObject.FindGameObjectWithTag("Player").transform; // Obtém a referência ao jogador
        playerController = player.GetComponent<Player>();

        if (moveDestination != null)
        {
            moveTarget = moveDestination.position;
        }
        else
        {
            moveTarget = startPosition + movePosition;
        }
        currentMoveDirection = (moveTarget - (Vector2)transform.position).normalized;
    }

    void Update()
    {
        if (playerAttacked)
        {
            // Começa a seguir o jogador se estiver dentro do alcance de perseguição
            if (Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                isChasing = true;
            }
        }

        if (isChasing)
        {
            ChasePlayer(); // Chama o método de perseguição
        }
        else
        {
            Move(); // Chama o método de movimento padrão do patinho
        }

        if (playerAttacked && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if (Time.time >= nextAttackTime)
            {
                AttackPlayer(); // Ataca o jogador se ele estiver dentro do alcance e o jogador tiver atacado primeiro
                nextAttackTime = Time.time + attackCooldown; // Define o tempo do próximo ataque
            }
        }
    }

    void Move()
    {
        if (!isReturning)
        {
            if (Vector2.Distance(transform.position, moveTarget) < 0.1f)
            {
                isReturning = true;
                currentMoveDirection = (startPosition - (Vector2)transform.position).normalized;
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, startPosition) < 0.1f)
            {
                isReturning = false;
                currentMoveDirection = (moveTarget - (Vector2)transform.position).normalized;
            }
        }

        transform.position += (Vector3)currentMoveDirection * speed * Time.deltaTime;
        FlipSprite(currentMoveDirection);
    }

    void ChasePlayer()
    {
        // Move o patinho em direção ao jogador
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Ajusta a rotação do sprite para a direção do movimento
        FlipSprite(direction);
    }

    void FlipSprite(Vector2 direction)
    {
        if (direction.x > 0)
        {
            // Virar sprite para a direita
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x < 0)
        {
            // Virar sprite para a esquerda
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Damage(int damage)
    {
        patinhoHealth -= damage; // Reduz a vida do patinho
        if (patinhoHealth <= 0)
        {
            Die(); // O patinho morre se a vida chegar a zero
        }
        else
        {
            playerAttacked = true; // O patinho só ataca se o jogador o atacar primeiro
        }
    }

    void AttackPlayer()
    {
        // Ataca o jogador se ele estiver dentro do alcance
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("O patinho está atacando o jogador!");
            playerController.Damage(patinhoDamage); // Causa dano ao jogador
        }
    }

    void Die()
    {
        Debug.Log("O patinho morreu!");
        Destroy(gameObject); // Destrói o patinho quando ele morre
    }

    private void OnDrawGizmos()
    {
        if (moveDestination != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, moveDestination.position);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPosition, startPosition + movePosition);
        }
    }
}
