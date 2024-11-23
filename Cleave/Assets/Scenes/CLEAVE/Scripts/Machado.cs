using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machado : MonoBehaviour
{
    public int attackDamage = 10; // Dano que a espada causará

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
    }
}
