using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flutuar : MonoBehaviour
{
    public float amplitude = 0.5f; // Altura do movimento
    public float frequency = 1f;  // Velocidade do movimento

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Armazena a posição inicial
    }

    void Update()
    {
        // Calcula o novo deslocamento vertical usando o seno
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;

        // Atualiza a posição do objeto
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}
