using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class apple : MonoBehaviour
{
    private bool _isCollected = false; // Evita múltiplas ativações acidentais

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que colidiu é o player
        if (collision.CompareTag("Player") && !_isCollected)
        {
            _isCollected = true;

            // Duplica as almas no GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.DoubleSouls(); // Método para duplicar as almas
                
            }

            // Destroi a maçã após ser coletada
            Destroy(gameObject);
        }
    }
}
