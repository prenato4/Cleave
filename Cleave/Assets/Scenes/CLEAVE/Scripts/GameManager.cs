using System;
using UnityEngine;

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

}