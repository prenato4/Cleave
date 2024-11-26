using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameboss2 : MonoBehaviour
{
    [Header("Configuração do Clotho")]
    public Image clothoHealthBarImage; // Referência à barra de vida do Clotho
    public Clotho clothoBoss;         // Referência ao script do Clotho

    void Start()
    {
        // Inicializa a barra de vida com os valores máximos
        UpdateHealthBars();
    }

    void Update()
    {
        // Atualiza a barra de vida em tempo real
        UpdateHealthBars();
    }

    void UpdateHealthBars()
    {
        if (clothoBoss != null && clothoHealthBarImage != null)
        {
            // Atualiza a barra de vida do Clotho
            float clothoFillAmount = (float)clothoBoss.currentHealth / clothoBoss.maxHealth;
            clothoHealthBarImage.fillAmount = Mathf.Clamp01(clothoFillAmount);
        }
    }
}