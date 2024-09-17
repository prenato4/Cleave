using System.Collections;
using UnityEngine;

public class Clotho : MonoBehaviour
{
    public int vidaMaxima = 100; // Vida máxima do boss
    public int vidaAtual; // Vida atual do boss
    public bool escudoAtivo = false; // Estado do escudo

    public float tempoEscudo = 5f; // Tempo que o escudo fica ativo
    public float intervaloEscudo = 10f; // Intervalo de tempo entre ativação do escudo

    private float tempoRestanteEscudo;
    private Animator an;

    public GameObject prefabAtaque; // Prefab do projétil a ser disparado
    public Transform pontoDisparo; // Ponto de onde o projétil será disparado
    public float intervaloDisparo = 3f; // Intervalo entre disparos

    private float tempoRestanteDisparo;

    public Transform Player; // Referência ao Transform do player
    public float gravidadeAlterada = 2f; // Gravidade alterada pelo boss
    public float velocidadeAlterada = 2f; // Velocidade alterada pelo boss
    public float tempoAlteracao = 5f; // Tempo de duração das alterações
    private float tempoRestanteAlteracao;
    private bool atacando = false;

    void Start()
    {
        an = GetComponent<Animator>();
        vidaAtual = vidaMaxima;
        tempoRestanteEscudo = intervaloEscudo;
        tempoRestanteDisparo = intervaloDisparo;

        StartCoroutine(AlterarGravidadeEVelocidadePeriodicamente());
        StartCoroutine(VerificarAtaques()); // Iniciar a rotina de ataque

        // Encontra o player na cena
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        // Atualiza o tempo restante do escudo
        tempoRestanteEscudo -= Time.deltaTime;

        // Ativa o escudo se o tempo restante for menor ou igual a 0
        if (tempoRestanteEscudo <= 0)
        {
            AtivarEscudo();
            tempoRestanteEscudo = intervaloEscudo; // Reinicia o tempo do escudo
        }

        // Atualiza o tempo restante para o próximo disparo
        tempoRestanteDisparo -= Time.deltaTime;

        // Dispara o projétil se o tempo restante for menor ou igual a 0
        if (tempoRestanteDisparo <= 0 && prefabAtaque != null)
        {
            DispararProjétil();
            tempoRestanteDisparo = intervaloDisparo; // Reinicia o tempo de disparo
        }

        // Atualiza o tempo restante para a alteração
        if (tempoRestanteAlteracao > 0)
        {
            tempoRestanteAlteracao -= Time.deltaTime;
            if (tempoRestanteAlteracao <= 0)
            {
                RestaurarPlayer();
            }
        }

        // Atualiza a vida e verifica se o boss morreu
        if (vidaAtual <= 0)
        {
            Morrer();
        }

        // Atualiza a rotação para o player constantemente
        FlipTowardsPlayer();
    }

    public void Damage(int dano)
    {
        if (!escudoAtivo) // Só recebe dano se o escudo não estiver ativo
        {
            vidaAtual -= dano;
            if (vidaAtual < 0) vidaAtual = 0; // Garante que a vida não seja negativa
        }
    }

    void AtivarEscudo()
    {
        escudoAtivo = true;
        an.SetInteger("Transition", 3); // Transição para o estado do escudo ativo
        // Ativa o escudo (pode adicionar efeitos visuais ou sonoros aqui)
        Invoke("DesativarEscudo", tempoEscudo); // Desativa o escudo após o tempo especificado
    }

    void DesativarEscudo()
    {
        escudoAtivo = false;
        an.SetInteger("Transition", 1); // Transição para o estado idle
        // Desativa o escudo (pode adicionar efeitos visuais ou sonoros aqui)
    }

    void DispararProjétil()
    {
        if (pontoDisparo != null && prefabAtaque != null)
        {
            atacando = true;
            an.SetInteger("Transition", 5); // Transição para o estado de ataque
            Instantiate(prefabAtaque, pontoDisparo.position, pontoDisparo.rotation);
        }
    }

    IEnumerator AlterarGravidadeEVelocidadePeriodicamente()
    {
        while (true)
        {
            if (Player != null)
            {
                // Altera a gravidade e a velocidade
                Player.GetComponent<Player>().SetGravity(gravidadeAlterada);
                Player.GetComponent<Player>().speed = velocidadeAlterada;

                // Define o tempo restante para a alteração
                tempoRestanteAlteracao = tempoAlteracao;

                an.SetInteger("Transition", 4); // Transição para o estado de alteração

                // Espera o tempo de alteração antes de restaurar
                yield return new WaitForSeconds(tempoAlteracao);

                // Restaura a gravidade e a velocidade do player
                RestaurarPlayer();

                an.SetInteger("Transition", 1); // Transição para o estado idle
            }

            // Espera 10 segundos antes da próxima alteração
            yield return new WaitForSeconds(10f);
        }
    }

    void RestaurarPlayer()
    {
        if (Player != null)
        {
            an.SetInteger("Transition", 1); // Transição para o estado idle
            Player.GetComponent<Player>().SetGravity(1f); // Restaura a gravidade padrão
            Player.GetComponent<Player>().speed = 5f; // Restaura a velocidade padrão
        }
    }

    void Morrer()
    {
        // Lógica para quando o boss morrer (ex. animação de morte, drop de itens, etc.)
        an.SetInteger("Transition", 6); // Transição para a animação de morte
        Destroy(gameObject, 1f); // Remove o boss do jogo após um pequeno atraso para a animação
    }

    IEnumerator VerificarAtaques()
    {
        while (true)
        {
            if (atacando)
            {
                // Retorna à animação padrão após o ataque
                yield return new WaitForSeconds(0.5f); // Aguarda um pouco antes de retornar ao idle
                an.SetInteger("Transition", 1); // Retorna ao estado idle
                atacando = false;
            }

            yield return null;
        }
    }

    void FlipTowardsPlayer()
    {
        if (Player != null)
        {
            // Verifica a posição do player para decidir a direção de ataque
            if (Player.position.x > transform.position.x)
            {
                // Player está à direita
                transform.localScale = new Vector3(1, 1, 1); // Olhando para a direita
            }
            else
            {
                // Player está à esquerda
                transform.localScale = new Vector3(-1, 1, 1); // Olhando para a esquerda
            }
        }
    }
}
