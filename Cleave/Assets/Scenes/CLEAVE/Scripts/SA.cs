using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA : MonoBehaviour
{
    public float bulletSpeed = 10f; // Velocidade da bala
    public int attackDamage = 1; // Dano causado pela bala
    private Rigidbody2D rb;
    private Vector2 bulletDirection; // Direção da bala

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = bulletDirection * bulletSpeed; // Aplica a velocidade da bala na direção passada
    }

    // Método para definir a direção da bala
    public void SetDirection(Vector2 direction)
    {
        bulletDirection = direction.normalized; // Normaliza a direção para evitar variações na velocidade
    }

    private void Update()
    {
        Destroy(gameObject, 2f); // Destruir a bala após 2 segundos
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se colidiu com o inimigo
        if (other.CompareTag("Inimigo")) 
        {
            Clotho enemy = other.GetComponent<Clotho>();
            
            if (enemy != null)
            {
                enemy.Damage(attackDamage); // Aplica o dano ao inimigo
            }
            
            Lanceiro e = other.GetComponent<Lanceiro>();
            
            if (e != null)
            {
                e.Damage(attackDamage); // Aplica o dano ao inimigo
            }
            
            roboescudo eee = other.GetComponent<roboescudo>();
            if (eee != null)
            {
                eee.Damage(attackDamage); // Aplica o dano ao inimigo
            }
            
            robotiro eeee = other.GetComponent<robotiro>();
            if (eeee != null)
            {
                eeee.Damage(attackDamage); // Aplica o dano ao inimigo
            }
            
            Fadinhas enem = other.GetComponent<Fadinhas>();
            if (enem != null)
            {
                enem.Damage(attackDamage); // Aplica o dano ao inimigo
            }
            Patinho ene = other.GetComponent<Patinho>();
            if (ene != null)
            {
                ene.Damage(attackDamage); // Aplica o dano ao inimigo
            }
            
            Hunt hunt = other.GetComponent<Hunt>();
            if (hunt != null)
            {
                hunt.Damage(attackDamage); // Aplica o dano ao inimigo
            }
            
            Erali Fada = other.GetComponent<Erali>();
            if (Fada != null)
            {
                Fada.Damage(attackDamage); // Aplica o dano ao inimigo
            }
            
            fadas fada = other.GetComponent<fadas>();
            if (fada != null)
            {
                fada.Damage(attackDamage); // Aplica o dano ao inimigo
            }
            
            Coelho coelho = other.GetComponent<Coelho>();
            if (coelho != null)
            {
                coelho.Damage(attackDamage); // Aplica o dano ao inimigo
            }
            
            Fungo fungo = other.GetComponent<Fungo>();
            if (fungo != null)
            {
                fungo.Damage(attackDamage); // Aplica o dano ao inimigo
            }
            
        }

        Destroy(gameObject); // Destruir a bala ao colidir
    }
}