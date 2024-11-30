using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tirohunter : MonoBehaviour
{
   public float bulletSpeed = 10f; // Velocidade da bala
    public int attackDamage = 4; // Dano causado pela bala
    private Rigidbody2D rb;
    private Vector2 bulletDirection; // Direção da bala

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = bulletDirection * bulletSpeed; // Aplica a velocidade da bala na direção passada
    }

    // Método para definir a direção da bala
    public void SetDirection(Vector2 direction)
    {
        bulletDirection = direction.normalized; // Normaliza a direção para evitar variações na velocidade
    }

    private void Update()
    {
        Destroy(gameObject, 2f); // Destruir a bala após 2 segundos
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se colidiu com o inimigo
        if (other.CompareTag("Player")) 
        {
            Player enemy = other.GetComponent<Player>();
            
            if (enemy != null)
            {
                enemy.Damage(attackDamage); // Aplica o dano ao inimigo
            }
            
        }

        Destroy(gameObject); // Destruir a bala ao colidir
    }
}
