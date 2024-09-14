using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    public Player player;
    public Coração vidaBarUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (player != null && vidaBarUI != null)
        {
            player.OnLifeChanged += vidaBarUI.UpdateLifeBar;
        }
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void prnivel()
    {
        SceneManager.LoadScene(1);
    }

}