using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alma : MonoBehaviour
{
    
    public float cura = 10f;  // Quantidade de cura ao coletar a alma
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            // Chama o método de cura no Player
            collision.GetComponent<Player>().Curar(cura);
            
            // Atualiza o contador de almas no GameManager
            GameManager.Instance.AddSoul();

            // Destroi a alma
            Destroy(gameObject);
        }
    }
}
