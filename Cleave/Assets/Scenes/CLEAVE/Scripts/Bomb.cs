using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{
    public float tempoAntesExplosao = 2f; // Tempo antes da bomba explodir
    public float raioExplosao = 5f; // Raio de dano da explosão
    public int dano = 10; // Dano causado pela explosão
    public LayerMask camadaDano; // Camada de objetos que recebem dano
    private Animator anim;

    private bool explodiu = false; // Evita múltiplas explosões


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Inicia uma coroutine para esperar antes de explodir
        if (!explodiu)
        {
            StartCoroutine(AguardaExplosao());
        }
    }

    private IEnumerator AguardaExplosao()
    {
        yield return new WaitForSeconds(tempoAntesExplosao); // Espera pelo tempo definido
        Explodir(); // Chama o método de explosão
    }

    private void Explodir()
    {
        
        anim.SetTrigger("bom");
        if (explodiu) return; // Evita múltiplas explosões
        explodiu = true;
        Debug.Log("A bomba explodiu!");

        // Encontrar objetos no raio da explosão
        Collider2D[] objetosAtingidos = Physics2D.OverlapCircleAll(transform.position, raioExplosao, camadaDano);

        foreach (Collider2D objeto in objetosAtingidos)
        {
            // Aplica dano ao objeto se ele tiver um componente de vida
            Player vida = objeto.GetComponent<Player>();
            if (vida != null)
            {
                vida.Damage(dano);
            }
        }

        Destroy(gameObject, 1f); // Destrói a bomba
    }

    private void OnCollisionEnter2D(Collision2D colisao)
    {
        // Se colidir com o player, explode imediatamente
        if (colisao.gameObject.CompareTag("Player"))
        {
            Explodir();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Se colidir com um trigger do player, explode imediatamente
        if (collider.CompareTag("Player"))
        {
            Explodir();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha o raio da explosão no editor para visualização
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioExplosao);
    }
}
