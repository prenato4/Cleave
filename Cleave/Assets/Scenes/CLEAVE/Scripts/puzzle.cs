using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzle : MonoBehaviour
{
    public int keysCollected = 0; // Quantidade de chaves coletadas
    public Engrenagens1[] gears; // Array de engrenagens no puzzle
    public GameObject targetObject; // Objeto que será ativado
    private bool hasToggled = false; // Variável para controlar se a alternância já ocorreu

    private void Update()
    {
        if (AreAllGearsActive())
        {
            ActivateTargetObject();
        }
    }

    // Verifica se todas as engrenagens estão ativas
    private bool AreAllGearsActive()
    {
        foreach (Engrenagens1 gear in gears)
        {
            if (!gear.IsActive)
                return false;
        }

        return true;
    }

    // Ativa o objeto final
    private void ActivateTargetObject()
    {
        if (!hasToggled) // Verifica se a alternância já foi feita
        {
            targetObject.SetActive(!targetObject.activeSelf);
            Debug.Log("Objeto alterado para: " + (targetObject.activeSelf ? "Ativado" : "Desativado"));
            hasToggled = true; // Marca que a alternância ocorreu
        }
    }
}
