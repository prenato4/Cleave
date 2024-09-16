using UnityEngine;

public class Flecha : MonoBehaviour
{
    public float speed = 10f; // Velocidade da flecha
    private Vector2 direction;

    // Método para definir a direção da flecha
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    void Update()
    {
        // Mover a flecha na direção definida
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Adicionar comportamento ao colidir (destruir a flecha, aplicar dano, etc.)
        Destroy(gameObject);
    }
}
