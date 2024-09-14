using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coelho : MonoBehaviour
{
    public float velocidade = 2f;
    public int vida = 3;
    public int danoAtaque = 1;
    public float alcanceAtaque = 1f;
    public float distanciaDeteccao = 5f; // Distância para o coelho começar a seguir o jogador
    public float tempoInvulnerabilidade = 0.5f; // Reduzido para ataques mais rápidos
    public float tempoCooldown = 0.5f; // Tempo de cooldown menor
    public float forcaPuloMin = 5f; // Força mínima do pulo
    public float forcaPuloMax = 10f; // Força máxima do pulo, tornando os saltos imprevisíveis

    private Transform jogador;
    private bool invulneravel = false;
    private bool atacando = false;
    private bool emCooldown = false;
    private float contadorAtaque = 0f;
    private float contadorCooldown = 0f;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (emCooldown)
        {
            // Se o coelho está em cooldown, ele não pode se mover
            contadorCooldown += Time.deltaTime;
            if (contadorCooldown >= tempoCooldown)
            {
                emCooldown = false;
                spriteRenderer.color = Color.white; // Volta à cor normal
            }
            return;
        }

        if (!atacando)
        {
            MoverParaJogador();
        }
        else
        {
            contadorAtaque += Time.deltaTime;
            if (contadorAtaque >= tempoInvulnerabilidade)
            {
                atacando = false;
                invulneravel = false;
                spriteRenderer.color = Color.white; // Volta à cor normal
                contadorAtaque = 0f;
                emCooldown = true;
                spriteRenderer.color = Color.blue; // Indica cooldown (opcional)
            }
        }
    }

    private void MoverParaJogador()
    {
        if (jogador != null)
        {
            // Verifica se o jogador está dentro da área de detecção
            float distanciaDoJogador = Vector2.Distance(transform.position, jogador.position);
            if (distanciaDoJogador <= distanciaDeteccao)
            {
                Vector2 direcao = (jogador.position - transform.position).normalized;
                //transform.position = Vector2.MoveTowards(transform.position, jogador.position, velocidade * Time.deltaTime);

                if (distanciaDoJogador <= alcanceAtaque)
                {
                    Atacar();
                }
            }
        }
    }

    private void Atacar()
    {
        atacando = true;
        invulneravel = true;
        spriteRenderer.color = Color.red; // Muda a cor para indicar invulnerabilidade

        // Calcula a direção para o jogador com variação aleatória
        Vector2 direcaoAleatoria = new Vector2(
            (jogador.position.x - transform.position.x) + Random.Range(-1f, 1f), // Aleatoriza a direção
            (jogador.position.y - transform.position.y) + Random.Range(-1f, 1f)
        ).normalized;

        // Aplica uma força com variação aleatória para o pulo
        float forcaPulo = Random.Range(forcaPuloMin, forcaPuloMax);
        rb.AddForce(direcaoAleatoria * forcaPulo, ForceMode2D.Impulse); // Simula o pulo em direção ao jogador

        // Causa dano ao jogador (você pode melhorar isso para detectar colisão durante o pulo)
        jogador.GetComponent<Player>().Damage(danoAtaque);
    }

    public void ReceberDano(int dano)
    {
        if (!invulneravel)
        {
            vida -= dano;

            if (vida <= 0)
            {
                Morrer();
            }
        }
    }

    private void Morrer()
    {
        Destroy(gameObject);
    }
}
