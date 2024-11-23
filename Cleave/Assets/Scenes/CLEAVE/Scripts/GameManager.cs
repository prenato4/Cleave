using UnityEngine;
using UnityEngine.UI; // Necessário para manipular textos no Canvas

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton para acesso global
    private int soulCount; // Contador de almas
    
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
            soulText.text = "Souls: " + soulCount;
        }
    }

    // Método para obter o número atual de almas (se necessário)
    public int GetSoulCount()
    {
        return soulCount;
    }
}