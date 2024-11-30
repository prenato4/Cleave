using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obejtodff : MonoBehaviour
{
    public GameObject objeto; // Referência ao objeto que será ativado/desativado

    // Método que será chamado para ativar o objeto
    public void AtivarObjeto()
    {
        objeto.SetActive(true);
    }

    // Método que será chamado para desativar o objeto
    public void DesativarObjeto()
    {
        objeto.SetActive(false);
    }
}
