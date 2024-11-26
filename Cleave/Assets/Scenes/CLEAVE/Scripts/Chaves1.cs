using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaves1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Obtém o gerenciador do puzzle
            Puzzle1 puzzleManager = FindObjectOfType<Puzzle1>();
            if (puzzleManager != null)
            {
                // Incrementa o número de chaves coletadas
                puzzleManager.keysCollected++;
                Debug.Log("Chave coletada! Total: " + puzzleManager.keysCollected);
                // Remove a chave da cena
                Destroy(gameObject);
            }
        }
        
        if (collision.CompareTag("Player"))
        {
            // Obtém o gerenciador do puzzle
            puzzle puzzleManager = FindObjectOfType<puzzle>();
            if (puzzleManager != null)
            {
                // Incrementa o número de chaves coletadas
                puzzleManager.keysCollected++;
                Debug.Log("Chave coletada! Total: " + puzzleManager.keysCollected);
                // Remove a chave da cena
                Destroy(gameObject);
            }
        }
    }
}
