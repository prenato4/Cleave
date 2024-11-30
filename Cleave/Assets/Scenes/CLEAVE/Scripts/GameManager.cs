using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necessário para manipular textos no Canvas

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton para acesso global
    private int soulCount; // Contador de almas
    
    public string currentLevelName; // Nome da fase atual

    public GameObject gamerover;
    
    
    public Player player;
    public Coração vidaBarUI;

    [SerializeField] private Text soulText; // Arraste o componente de texto do Canvas para cá no Inspector

    private void Awake()
    {
        // Configura o singleton
        if (Instance == null)
        {
            Instance = this;
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
    
    void Update()
    {
        
    }

    // Método para adicionar almas
    public void AddSoul()
    {
        soulCount++;
        UpdateSoulUI(); // Atualiza a interface
        Debug.Log("Souls collected: " + soulCount);
    }

    // Método para duplicar as almas
    public void DoubleSouls()
    {
        soulCount *= 2;
        UpdateSoulUI(); // Atualiza a interface
        Debug.Log("Souls doubled! New total: " + soulCount);
    }

    // Método para atualizar a interface das almas
    private void UpdateSoulUI()
    {
        if (soulText != null)
        {
            soulText.text = ": " + soulCount;
        }
    }

    // Método para obter o número atual de almas (se necessário)
    public int GetSoulCount()
    {
        return soulCount;
    }
    public void GameOver()
    {
        gamerover.SetActive(true);
    }
    

    

    public void UpdateCurrentLevel(string levelName)
    {
        currentLevelName = levelName; // Atualiza a fase atual
    }
}