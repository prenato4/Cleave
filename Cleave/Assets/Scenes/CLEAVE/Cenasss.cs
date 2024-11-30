using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cenasss : MonoBehaviour
{
    
    public GameObject objectToToggle; // O objeto que será ativado/desativado
    public GameObject gameOverPanel; // Arraste o painel com o botão no Inspector

    private void Start()
    {
        // Desativa o painel de Game Over quando a cena iniciar
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Alterna o estado de ativação do objeto
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }
    }
    

    
    public void LoadFase1()
    {
        SceneManager.LoadScene("Fase 1");
    }
    
    public void LoadFase2()
    {
        SceneManager.LoadScene("Fase 2");
    }
    
    public void LoadBoss1()
    {
        SceneManager.LoadScene("Boss 1");
    }
    
    public void LoadBoss2()
    {
        SceneManager.LoadScene("Boss 2");
    }
    
    public void LoadCena2()
    {
        SceneManager.LoadScene("cutscene2");
    }
    
    public void LoadCena3()
    {
        SceneManager.LoadScene("cutscene3");
    }
    
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene("Fase 1");
    }
}
