using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1 : MonoBehaviour
{
    public int keysCollected = 0; // Quantidade de chaves coletadas
    public Engrenagens1[] gears; // Array de engrenagens no puzzle
    public GameObject targetObject; // Objeto que será ativado

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
        targetObject.SetActive(true);
        Debug.Log("Objeto ativado!");
    }
}
