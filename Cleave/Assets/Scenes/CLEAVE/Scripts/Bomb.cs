using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{
    public float tempoAntesExplosao = 2f; // Tempo antes da bomba explodir

    private void Update()
    {
        // Inicia uma coroutine para esperar antes de explodir
        StartCoroutine(AguardaExplosao());
    }

    private IEnumerator AguardaExplosao()
    {
        yield return new WaitForSeconds(tempoAntesExplosao); // Espera pelo tempo definido
        Explodir(); // Chama o método de explosão
    }

    private void Explodir()
    {

        Debug.Log("A bomba explodiu!");
        Destroy(gameObject); // Destrói a bomba
    }

    private void OnCollisionEnter2D(Collision2D colisao)
    {
        // Se colidir com qualquer objeto, explode
        //Explodir();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Se colidir com um trigger, explode
        //Explodir();
    }
}