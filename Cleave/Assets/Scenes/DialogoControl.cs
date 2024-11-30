using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogoControl : MonoBehaviour
{
    public GameObject dialogueUI; 
    private int currentLineIndex = 0; 
    private bool isPlayerNearby = false;

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            dialogueUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            dialogueUI.SetActive(false);
        }
    }
}