using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int soulCount;
    public string currentLevelName;
    

    public Player player;
    public Coração vidaBarUI;

    [SerializeField] private Text soulText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Adicione o evento de carregamento de cena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeComponents();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        player = FindObjectOfType<Player>(); // Encontra o Player na cena atual
        vidaBarUI = FindObjectOfType<Coração>(); // Encontra o componente da barra de vida
        soulText = FindObjectOfType<Text>();
        if (player != null && vidaBarUI != null)
        {
            player.OnLifeChanged -= vidaBarUI.UpdateLifeBar; // Remove listeners duplicados
            player.OnLifeChanged += vidaBarUI.UpdateLifeBar; // Adiciona novamente o listener
        }

        // Atualiza a interface de almas
        UpdateSoulUI();
    }

    private void UpdateSoulUI()
    {
        if (soulText != null)
        {
            soulText.text = ": " + soulCount;
        }
    }

    public void AddSoul()
    {
        soulCount++;
        UpdateSoulUI();
        Debug.Log("Souls collected: " + soulCount);
    }

    public void DoubleSouls()
    {
        soulCount *= 2;
        UpdateSoulUI();
        Debug.Log("Souls doubled! New total: " + soulCount);
    }
    

    public void UpdateCurrentLevel(string levelName)
    {
        currentLevelName = levelName;
    }
}
