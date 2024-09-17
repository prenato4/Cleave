using UnityEngine;

public class Erali : MonoBehaviour
{
    public Hunt huntBoss;  // Referência para o script de vida do Hunt
    public int healingAmount = 20;  // Quantidade de vida que Erali vai curar
    public float healInterval = 10f;  // Intervalo de 10 segundos para curar

    private float healTimer;  // Temporizador para controlar o tempo de cura
    
    public int maxHealth = 100;  // Vida máxima da Erali
    public int currentHealth;    // Vida atual da Erali
    
    
    public float speed = 2f; // Velocidade de movimento da Erali
    public Vector2 movePosition; // Distância máxima que a Erali se move
    public Transform moveDestination; // Ponto final para o movimento da Erali

    private Vector2 startPosition; // Posição inicial da Erali
    private Vector2 moveTarget; // Ponto alvo para o movimento
    private Vector2 currentMoveDirection; // Direção atual do movimento
    private bool isReturning = false; // Indica se a Erali está voltando ao ponto inicial

    void Start()
    {
        
        currentHealth = maxHealth;          // Define a vida inicial como o valor máximo
        
        healTimer = healInterval;  // Inicia o temporizador
        
        startPosition = transform.position; // Define a posição inicial da Erali

        if (moveDestination != null)
        {
            moveTarget = moveDestination.position; // Define o destino de movimento se houver um ponto especificado
        }
        else
        {
            moveTarget = startPosition + movePosition; // Caso contrário, usa uma posição calculada com base na posição inicial
        }

        currentMoveDirection = (moveTarget - (Vector2)transform.position).normalized; // Calcula a direção de movimento
    }

    void Update()
    {
        healTimer -= Time.deltaTime;  // Reduz o tempo do temporizador

        // Quando o tempo chegar a zero, cura o Hunt e reinicia o temporizador
        if (healTimer <= 0f)
        {
            HealHunt();
            healTimer = healInterval;  // Reinicia o temporizador
        }
        
        Move();
    }
    
    void Move()
    {
        if (!isReturning)
        {
            // Se chegar ao alvo, começa a retornar
            if (Vector2.Distance(transform.position, moveTarget) < 0.1f)
            {
                isReturning = true;
                currentMoveDirection = (startPosition - (Vector2)transform.position).normalized;
            }
        }
        else
        {
            // Se chegar ao ponto inicial, vai de volta para o alvo
            if (Vector2.Distance(transform.position, startPosition) < 0.1f)
            {
                isReturning = false;
                currentMoveDirection = (moveTarget - (Vector2)transform.position).normalized;
            }
        }

        // Movimenta a Erali
        transform.position += (Vector3)currentMoveDirection * speed * Time.deltaTime;

        // Chama o método para virar o sprite dependendo da direção
        FlipSprite(currentMoveDirection);
    }
    
    void FlipSprite(Vector2 direction)
    {
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Virar para a direita
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Virar para a esquerda
        }
    }
    
    public void Damage(int damage)
    {
        currentHealth -= damage; // Reduz a vida atual
        Debug.Log("Erali recebeu " + damage + " de dano. Vida atual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); // Chama o método de morte se a vida chegar a zero
        }
    }

    // Método para desenhar o caminho de movimento no editor
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

    // Função para curar o Hunt diretamente acessando a variável currentHealth
    void HealHunt()
    {
        if (huntBoss != null)
        {
            huntBoss.currentHealth += healingAmount;  // Aumenta a vida diretamente

            // Garante que a vida não ultrapasse o máximo
            if (huntBoss.currentHealth > huntBoss.maxHealth)
            {
                huntBoss.currentHealth = huntBoss.maxHealth;
            }

            Debug.Log("Erali curou Hunt em " + healingAmount + " de vida! Vida atual: " + huntBoss.currentHealth);
        }
    }
    
    void Die()
    {
        // Desativar ou destruir o objeto da Erali
        Destroy(gameObject); 
    }
}