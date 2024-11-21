using System;
using System.Collections;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float trapDuration = 2f; // Tempo que o jogador ficará preso
    public float explosionRadius = 2f; // Raio da explosão
    public int explosionDamage = 10; // Dano da explosão
    public GameObject explosionEffect; // Efeito visual da explosão

    private bool isTriggered = false;
    private Animator animator;

    private void Start()
    {
        // Obtém o componente Animator da própria armadilha
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(ActivateTrap(other.gameObject));
        }
    }

    private IEnumerator ActivateTrap(GameObject player)
    {
        // Ativar a animação da armadilha
        if (animator != null)
        {
            animator.SetTrigger("nnn"); // "nnn" deve ser o nome do trigger configurado no Animator da armadilha
        }

        // Prender o jogador (exemplo: desativar o controle do jogador)
        var playerController = player.GetComponent<Player>();
        if (playerController != null)
        {
            playerController.DisableControls();
        }

        // Esperar 2 segundos
        yield return new WaitForSeconds(trapDuration);

        // Explodir a armadilha
        Explode();

        // Restaurar o controle do jogador
        if (playerController != null)
        {
            playerController.EnableControls();
        }

        // Destruir a armadilha
        Destroy(gameObject);
    }

    private void Explode()
    {
        // Criar efeito de explosão
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Aplicar dano na área da explosão
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // Aplicar dano ao jogador
                var playerHealth = hitCollider.GetComponent<Player>();
                if (playerHealth != null)
                {
                    playerHealth.Damage(explosionDamage);
                }
            }
        }
    }
}
