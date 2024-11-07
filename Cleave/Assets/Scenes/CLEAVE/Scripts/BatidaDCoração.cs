using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatidaDCoração : MonoBehaviour
{
    
    public float minScale = 1f;  // Tamanho mínimo do coração
    public float maxScale = 1.5f; // Tamanho máximo do coração
    public float beatSpeed = 2f; // Velocidade do batimento (quanto maior, mais rápido)
    
    private Vector3 initialScale;
    
    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale; // Armazena o tamanho inicial do coração
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula o fator de escala, fazendo uma animação de batimento
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(Time.time * beatSpeed, 1));
        
        // Aplica o novo tamanho
        transform.localScale = initialScale * scale;
    }
}
