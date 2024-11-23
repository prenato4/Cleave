using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engrenagens1 : MonoBehaviour
{
    public bool IsActive { get; private set; } = false; // Status da engrenagem
    private Puzzle1 puzzleManager;

    private void Start()
    {
        puzzleManager = FindObjectOfType<Puzzle1>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !IsActive && puzzleManager.keysCollected > 0)
        {
            ActivateGear();
        }
    }

    private void ActivateGear()
    {
        IsActive = true;
        puzzleManager.keysCollected--;
        Debug.Log("Engrenagem ativada! Chaves restantes: " + puzzleManager.keysCollected);
        // Aqui você pode adicionar uma animação ou efeito visual para a engrenagem
    }
}
