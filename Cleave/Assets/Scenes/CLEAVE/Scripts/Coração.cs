using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coração : MonoBehaviour
{
    
    public Image vida; // A barra de vida no Canvas
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateLifeBar(int currentLife)
    {
        if (vida != null)
        {
            // Assumindo que maxLife é 100
            float maxLife = 100f; // Se a vida máxima for diferente, ajuste aqui
            float fillAmount = Mathf.Clamp01(currentLife / maxLife); // Calcula o valor de fillAmount
            vida.fillAmount = fillAmount;
        }
    }
    
}
