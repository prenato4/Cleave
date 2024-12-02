using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class boss11 : MonoBehaviour
{
    public GameObject objectToActivate; // O objeto que será ativado
    private int enemiesDestroyed = 0;   // Contador de inimigos destruídos
    
    // Esta função será chamada sempre que um inimigo for destruído
    public void EnemyDestroyed()
    {
        enemiesDestroyed++; // Aumenta o contador

        // Verifica se 2 inimigos foram destruídos
        if (enemiesDestroyed >= 2)
        {
            ActivateObject(); // Ativa o objeto
        }
    }

    // Função que ativa o objeto
    void ActivateObject()
    {
        SceneManager.LoadScene("cutscene2");
    }
    
}
