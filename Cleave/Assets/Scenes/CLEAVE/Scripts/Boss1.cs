using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1 : MonoBehaviour
{
    [Header("Configuração do Hunt")]
    public Image huntHealthBarImage; // Referência à barra de vida do Hunt
    public Hunt huntBoss;            // Referência ao script do Hunt

    [Header("Configuração da Erali")]
    public Image eraliHealthBarImage; // Referência à barra de vida da Erali
    public Erali erali;               // Referência ao script da Erali

    void Start()
    {
        // Inicializa as barras de vida com os valores máximos
        UpdateHealthBars();
    }

    void Update()
    {
        // Atualiza as barras de vida em tempo real
        UpdateHealthBars();
    }

    void UpdateHealthBars()
    {
        if (huntBoss != null && huntHealthBarImage != null)
        {
            // Atualiza a barra de vida do Hunt
            float huntFillAmount = (float)huntBoss.currentHealth / huntBoss.maxHealth;
            huntHealthBarImage.fillAmount = Mathf.Clamp01(huntFillAmount);
        }

        if (erali != null && eraliHealthBarImage != null)
        {
            // Atualiza a barra de vida da Erali
            float eraliFillAmount = (float)erali.currentHealth / erali.maxHealth;
            eraliHealthBarImage.fillAmount = Mathf.Clamp01(eraliFillAmount);
        }
    }
    
    
}
