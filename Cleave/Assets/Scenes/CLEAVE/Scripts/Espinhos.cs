using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int damage = 10; // Quantidade de dano causada pelo espinho

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Verifica se colidiu com o jogador
        {
            Player player = collision.gameObject.GetComponent<Player>(); // Obtém o componente Player do jogador

            if (player != null)
            {
                player.Damage(damage); // Chama a função TakeDamage do jogador passando a quantidade de dano
            }
        }
    }
}