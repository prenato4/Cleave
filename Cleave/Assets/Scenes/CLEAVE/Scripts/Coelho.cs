using System.Collections;
using UnityEngine;

public class Coelho : MonoBehaviour
{
    public float velocidade = 2f;
    public int vida = 3;
    public int danoAtaque = 1;
    public float alcanceAtaque = 1f;
    public float distanciaDeteccao = 5f; 
    public float tempoCooldown = 0.5f;
    public float forcaPuloMin = 5f;
    public float forcaPuloMax = 10f;
    public float delayAtaque = 0.2f;
    public float duracaoAnimacaoAtaque = 0.3f; // Duração da animação de ataque

    private Transform jogador;
    private bool atacando = false;
    private bool emCooldown = false;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    private float contadorCooldown = 0f;
    
    public GameObject soulPrefab; // Prefab da alma que será dropada

    private void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (emCooldown)
        {
            contadorCooldown += Time.deltaTime;
            if (contadorCooldown >= tempoCooldown)
            {
                emCooldown = false;
                contadorCooldown = 0f;
                spriteRenderer.color = Color.white;
            }
            return;
        }

        float distanciaDoJogador = Vector2.Distance(transform.position, jogador.position);

        if (distanciaDoJogador <= distanciaDeteccao && distanciaDoJogador > alcanceAtaque)
        {
            MoverParaJogador();
        }
        else if (distanciaDoJogador <= alcanceAtaque)
        {
            Atacar();
        }
        else
        {
            Idle();
        }
        
        // Gira o coelho para olhar na direção do jogador
        if (jogador != null)
        {
            if (jogador.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void Idle()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
    }

    private void MoverParaJogador()
    {
        Vector2 direcao = (jogador.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, jogador.position, velocidade * Time.deltaTime);

        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    private void Atacar()
    {
        if (!atacando)
        {
            atacando = true;
            emCooldown = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);

            StartCoroutine(ExecutarAtaque());
        }
    }

    private IEnumerator ExecutarAtaque()
    {
        yield return new WaitForSeconds(delayAtaque); // Pausa para a animação

        Vector2 direcaoAleatoria = new Vector2(
            (jogador.position.x - transform.position.x) + Random.Range(-1f, 1f),
            (jogador.position.y - transform.position.y) + Random.Range(-1f, 1f)
        ).normalized;

        float forcaPulo = Random.Range(forcaPuloMin, forcaPuloMax);
        rb.AddForce(direcaoAleatoria * forcaPulo, ForceMode2D.Impulse);

        jogador.GetComponent<Player>().Damage(danoAtaque);

        yield return new WaitForSeconds(duracaoAnimacaoAtaque); // Duração da animação de ataque
        animator.SetBool("isAttacking", false); // Finaliza a animação de ataque
        
        atacando = false;
    }

    public void Damage(int dano)
    {
        vida -= dano;
        if (vida <= 0)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        // Instantiate a alma no local do inimigo
        Instantiate(soulPrefab, transform.position, Quaternion.identity);

        // Notifica o GameManager
        GameManager.Instance.AddSoul();
        
        Destroy(gameObject);
    }
}
