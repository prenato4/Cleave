using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogos : MonoBehaviour
{
    public GameObject objectToToggle; // Objeto a ser ativado ou desativado

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto que entrou no trigger é o player
        if (other.CompareTag("Player"))
        {
            objectToToggle.SetActive(true); // Ativa o objeto
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Verifica se o objeto que saiu do trigger é o player
        if (other.CompareTag("Player"))
        {
            objectToToggle.SetActive(false); // Desativa o objeto
        }
    }
}
