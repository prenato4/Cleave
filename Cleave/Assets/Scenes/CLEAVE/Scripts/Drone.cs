using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public float speed = 2f; // Velocidade de movimento do drone
    public Vector2 movePosition; // Distância máxima que o drone se move
    public Transform moveDestination; // Ponto final para o movimento do drone

    private Vector2 startPosition; // Posição inicial do drone
    private Vector2 moveTarget; // Ponto alvo para o movimento
    private Vector2 currentMoveDirection; // Direção atual do movimento
    private bool isReturning = false; // Indica se o drone está voltando ao ponto inicial

    public int droneHealth = 5; // Vida do drone
    public GameObject objetoParaInstanciar; // Objeto que será instanciado
    public float intervaloInstanciacao = 10f; // Intervalo de tempo para instanciar o objeto
    private float tempoInstanciacaoAtual = 0f; // Controla o tempo de espera para instanciar

    void Start()
    {
        startPosition = transform.position; // Define a posição inicial do drone

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
        // Chama o método de movimento padrão do drone
        Move();

        // Controla o tempo de espera para instanciar o objeto
        tempoInstanciacaoAtual += Time.deltaTime;
        if (tempoInstanciacaoAtual >= intervaloInstanciacao)
        {
            InstanciarObjeto();
            tempoInstanciacaoAtual = 0f;
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

    void FlipSprite(Vector2 direction)
    {
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void Damage(int damage)
    {
        droneHealth -= damage; // Reduz a vida do drone

        if (droneHealth <= 0)
        {
            Die(); // O drone morre se a vida chegar a zero
        }
    }

    void Die()
    {
        Debug.Log("O drone foi destruído!");
        Destroy(gameObject); // Destrói o drone quando ele morre
    }

    void InstanciarObjeto()
    {
        if (objetoParaInstanciar != null)
        {
            Instantiate(objetoParaInstanciar, transform.position, Quaternion.identity); // Instancia o objeto na posição do drone
            Debug.Log("Objeto instanciado pelo drone!");
        }
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
