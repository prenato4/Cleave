using System.Collections;
using UnityEngine;

public class Erali : MonoBehaviour
{
    public Hunt huntBoss;  // Referência para o script de vida do Hunt
    public int healingAmount = 20;  // Quantidade de vida que Erali vai curar
    public float healInterval = 10f;  // Intervalo de 10 segundos para curar
    public GameObject healingParticles; // Referência para o sistema de partículas de cura

    private float healTimer;  // Temporizador para controlar o tempo de cura
    
    public int maxHealth = 100;  // Vida máxima da Erali
    public int currentHealth;    // Vida atual da Erali
    public float objectActiveDuration = 5f;
    public GameObject objectToActivate;  // Objeto a ser ativado
    public GameObject spawnPrefab;       // Prefab do objeto a ser instanciado
    public float spawnInterval = 0.5f;   // Intervalo entre cada instância
    public float spawnDelay = 20f;       // Tempo entre cada grupo de instâncias

    private float spawnTimer;
    public boss11 enemyManager; // Referência ao EnemyManager
    
    
    public float speed = 2f; // Velocidade de movimento da Erali
    public Vector2 movePosition; // Distância máxima que a Erali se move
    public Transform moveDestination; // Ponto final para o movimento da Erali

    private Vector2 startPosition; // Posição inicial da Erali
    private Vector2 moveTarget; // Ponto alvo para o movimento
    private Vector2 currentMoveDirection; // Direção atual do movimento
    private bool isReturning = false; // Indica se a Erali está voltando ao ponto inicial

    void Start()
    {
        spawnTimer = spawnDelay;  
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
        FollowHunt();
        //Move();
        
        
        // Contagem regressiva para instanciar o próximo grupo de objetos
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            ActivateObject();
            spawnTimer = spawnDelay;  // Reinicia o temporizador para o próximo grupo
        }
    }
    
    void OnDestroy()
    {
        // Quando o inimigo for destruído, chama a função no EnemyManager
        if (enemyManager != null)
        {
            enemyManager.EnemyDestroyed(); // Informa que o inimigo foi destruído
        }
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
            transform.localScale = new Vector3(-1, 1, 1); // Virar para a direita
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Virar para a esquerda
        }
    }
    
    
    
    void ActivateObject()
    {
        // Ativa o objeto desativado
        if (objectToActivate != null)
        {
            ActivateHealingParticles();
            objectToActivate.SetActive(true);
        
            // Instancia as fadas no mesmo local onde o objeto é ativado
            StartCoroutine(SpawnObjectsAtPosition(objectToActivate.transform.position));
        
            StartCoroutine(DeactivateObjectAfterDelay(objectActiveDuration)); // Inicia a corrotina para desativar o objeto
        }
    }
    
    IEnumerator SpawnObjectsAtPosition(Vector3 position)
    {
        // Instancia um grupo de objetos com intervalo de spawnInterval na posição especificada
        for (int i = 0; i < 5; i++)  // Ajuste o número de instâncias conforme necessário
        {
            Instantiate(spawnPrefab, position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    IEnumerator DeactivateObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // Espera o tempo especificado
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);  // Desativa o objeto
        }
    }
    
    void FollowHunt()
    {
        if (huntBoss != null)
        {
            float distanceToHunt = Vector2.Distance(transform.position, huntBoss.transform.position);
            float minDistance = 2f; // Distância mínima desejada
            float fixedHeight = 1f; // Altura fixa da Erali

            // Segue o Hunt se estiver além da distância mínima
            if (distanceToHunt > minDistance)
            {
                Vector2 directionToHunt = (huntBoss.transform.position - transform.position).normalized;
                Vector3 targetPosition = transform.position + (Vector3)directionToHunt * speed * Time.deltaTime;

                // Define a altura fixa mantendo a posição Y constante
                targetPosition.y = fixedHeight;
                transform.position = targetPosition;

                // Chama o método para virar o sprite dependendo da direção
                AlignRotationWithHunt();
            }
        }
    }
    
    void AlignRotationWithHunt()
    {
        if (huntBoss != null)
        {
            // Ajusta a rotação no eixo Y para combinar com a direção do Hunt
            if (huntBoss.transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z); // Olhando para a direita
            }
            else
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z); // Olhando para a esquerda
            }
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
        // Verifica se o Hunt ainda está presente na cena
        if (huntBoss != null && huntBoss.gameObject.activeInHierarchy)
        {
            // Verifica se a vida do Hunt está abaixo do máximo
            if (huntBoss.currentHealth < huntBoss.maxHealth)
            {
                // Aumenta a vida do Hunt pela quantidade de cura
                huntBoss.currentHealth += healingAmount;

                // Garante que a vida do Hunt não ultrapasse o máximo
                if (huntBoss.currentHealth > huntBoss.maxHealth)
                {
                    huntBoss.currentHealth = huntBoss.maxHealth;
                }

                Debug.Log("Erali curou Hunt em " + healingAmount + " de vida! Vida atual: " + huntBoss.currentHealth);

                ActivateHealingParticles();
            }
        }
        else
        {
            Debug.Log("Hunt não está na cena. Erali não pode curá-lo.");
        }
    }
    
    void ActivateHealingParticles()
    {
        if (healingParticles != null)
        {
            // Instancia as partículas na posição da Erali
            GameObject particles = Instantiate(healingParticles, transform.position, Quaternion.identity);
        
            // Destrói as partículas após um tempo (ajuste o tempo conforme necessário)
            Destroy(particles, 2f);
        }
    }
    
    void Die()
    {
        // Desativar ou destruir o objeto da Erali
        Destroy(gameObject); 
    }
}