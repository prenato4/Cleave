using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pensa : MonoBehaviour
{
    public float dropSpeed = 2f;      // Velocidade de descida da prensa
    public float pressedHeight = 1f;  // Altura onde a prensa desce
    public float resetHeight = 10f;   // Altura onde a prensa começa
    public float delayBeforeReset = 2f; // Tempo que a prensa fica abaixada antes de voltar

    private bool isPressed = false;
    private bool hasBeenActivated = false; // Para garantir que a prensa desça apenas uma vez
    private Vector3 initialPosition;
    private float timer = 0f;

    void Start()
    {
        // Guarda a posição inicial da prensa
        initialPosition = transform.position;
    }

    void Update()
    {
        if (isPressed && !hasBeenActivated)
        {
            // Move a prensa para baixo até a altura desejada
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, pressedHeight, transform.position.z), Time.deltaTime * dropSpeed);

            // Verifica se a prensa chegou perto da posição final
            if (Vector3.Distance(transform.position, new Vector3(transform.position.x, pressedHeight, transform.position.z)) < 0.1f)
            {
                hasBeenActivated = true;
                timer = delayBeforeReset; // Inicia o temporizador
            }
        }
        else if (hasBeenActivated)
        {
            // Contador regressivo para a volta da prensa
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                // Move a prensa de volta para a altura inicial
                transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * dropSpeed);

                // Verifica se a prensa chegou perto da posição inicial
                if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
                {
                    // Para o movimento da prensa
                    transform.position = initialPosition;
                    hasBeenActivated = false;
                    isPressed = false;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto que entrou no trigger é o esperado
        if (other.CompareTag("Player")) // Troque "Player" pelo tag que você quiser usar
        {
            isPressed = true;
        }
    }
}
