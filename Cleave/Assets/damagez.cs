using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damageAmount = 5; // Quantidade de dano que o Trigger causa

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou no Trigger tem a tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Tenta acessar um script de vida no player
            Player playerHealth = collision.GetComponent<Player>();

            if (playerHealth != null)
            {
                // Aplica dano ao player
                playerHealth.Damage(damageAmount);
            }
        }
    }
}