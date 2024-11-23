using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class esporos : MonoBehaviour
{
    public float moveSpeed = 3f; // Velocidade de movimento dos esporos
    public float rotationSpeed = 200f; // Velocidade de rotação dos esporos
    public float lifespan = 5f; // Tempo de vida do esporo antes de ser destruído
    public int damage = 1;

    private Vector2 _moveDirection;

    void Start()
    {
        // Define a direção de movimento como uma direção aleatória
        float angle = Random.Range(0, 360f);
        _moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        // Destrói o esporo após o tempo de vida definido
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        // Move o esporo na direção definida
        transform.Translate(_moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Gira o esporo
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Verifica se o objeto colidido possui o componente "Player"
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            // Aplica dano ao jogador
            player.Damage(damage);
            Destroy(gameObject); // Destrói o esporo após causar dano
        }
    }
}
