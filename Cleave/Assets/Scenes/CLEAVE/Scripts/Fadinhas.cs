using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadinhas : MonoBehaviour
{
    public float speed = 2f; // Velocidade de movimento da fada
    public Vector2 movePosition; // Distância máxima que a fada se move
    public Transform moveDestination; // Ponto final para o movimento da fada

    private Vector2 startPosition; // Posição inicial da fada
    private Vector2 moveTarget; // Ponto alvo para o movimento
    private Vector2 currentMoveDirection; // Direção atual do movimento
    private bool isReturning = false; // Indica se a fada está voltando ao ponto inicial

    public bool playerAttacked = false; // Indica se o jogador atacou a fada
    public float attackRange = 2f; // Distância mínima para a fada atacar
    public float chaseRange = 5f; // Distância mínima para a fada começar a seguir o jogador
    public int fairyHealth = 5; // Vida da fada
    public int fairyDamage = 1; // Dano que a fada causa ao jogador
    public float attackCooldown = 1.5f; // Tempo entre os ataques da fada
    private float nextAttackTime = 0f; // Controla o tempo de espera entre ataques

    private Transform player;
    private Player playerController;
    private bool isChasing = false; // Indica se a fada está seguindo o jogador

    void Start()
    {
        startPosition = transform.position; // Define a posição inicial da fada
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
            if (Vector2.Distance(transform.position, player.position) <= chaseRange)
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
            Move(); // Chama o método de movimento padrão da fada
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
        // Move a fada em direção ao jogador
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
        fairyHealth -= damage; // Reduz a vida da fada
        if (fairyHealth <= 0)
        {
            Die(); // A fada morre se a vida chegar a zero
        }
        else
        {
            playerAttacked = true; // A fada só ataca se o jogador a atacar primeiro
        }
    }

    void AttackPlayer()
    {
        // Ataca o jogador se ele estiver dentro do alcance
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("A fada está atacando o jogador!");
            playerController.Damage(fairyDamage); // Causa dano ao jogador
        }
    }

    void Die()
    {
        Debug.Log("A fada morreu!");
        Destroy(gameObject); // Destrói a fada quando ela morre
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
