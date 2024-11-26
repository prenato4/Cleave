using System.Collections;
using UnityEngine;

public class Lanceiro : MonoBehaviour
{
    public float moveSpeed;
    public bool useTransform;
    public bool shouldFlip;
    public float attackRange = 2f; // Alcance para atacar o player
    public GameObject attackObjectPrefab; // Prefab do objeto de ataque

    [SerializeField] private Vector2 movePosition;
    [SerializeField] private Transform moveDestination;
    [SerializeField] private float detectionRange = 5f; // Alcance para detectar o player

    private Vector2 _initialPosition;
    private Vector2 _moveTarget;
    private Vector2 _currentMoveDirection;
    private bool _isReturning;
    private float _originalLocalScaleX;
    private Animator _animator;
    private Transform _player;
    
    public GameObject soulPrefab;

    // Sistema de Vida
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _originalLocalScaleX = Mathf.Abs(transform.localScale.x);

        if (useTransform)
        {
            _moveTarget = moveDestination.localPosition;
        }
        else
        {
            _moveTarget = movePosition;
        }

        _initialPosition = transform.position;
        _currentMoveDirection = (_initialPosition + _moveTarget - (Vector2)transform.position).normalized;

        _player = GameObject.FindGameObjectWithTag("Player").transform;

        // Inicializa a vida do Lanceiro
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die(); // Verifica se morreu
            return; // Não faz mais nada se estiver morto
        }

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (distanceToPlayer <= detectionRange)
        {
            FollowPlayer(); // Segue o player
        }
        else
        {
            Patrol(); // Continua patrulhando
        }
    }

    private void FollowPlayer()
    {
        if (_player == null) return;

        // Faz o lanceiro olhar para o player
        FacePlayer();

        // Verifica a distância para atacar
        float distanceToAttack = Vector2.Distance(transform.position, _player.position);

        if (distanceToAttack <= attackRange)
        {
            Attack(); // Ataca se estiver no alcance de ataque
        }
        else
        {
            // Move em direção ao player
            Vector2 directionToPlayer = (_player.position - transform.position).normalized;
            transform.position += (Vector3)directionToPlayer * moveSpeed * Time.deltaTime;
            _animator.SetBool("mo", true); // Ativa a animação de movimento
        }
    }

    private void Attack()
    {
        _animator.SetTrigger("Attack"); // Ativa a animação de ataque

        // Inicia o loop de ativação/desativação do objeto de ataque
        if (attackObjectPrefab != null)
        {
            StartCoroutine(AttackLoop());
        }
    }
    
    private IEnumerator AttackLoop()
    {
        // Liga o objeto de ataque
        attackObjectPrefab.SetActive(true);
        yield return new WaitForSeconds(1f); // Duração do objeto ativo (ajuste conforme necessário)

        // Desliga o objeto de ataque
        attackObjectPrefab.SetActive(false);
        yield return new WaitForSeconds(1f); // Duração do intervalo antes de ligar novamente
    }

    private void FacePlayer()
    {
        if (_player == null) return;

        float direction = _player.position.x - transform.position.x;

        // Ajusta a escala do lanceiro para encarar o player
        if (direction < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(_originalLocalScaleX), transform.localScale.y, transform.localScale.z);
        }
        else if (direction > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(_originalLocalScaleX), transform.localScale.y, transform.localScale.z);
        }
    }

    private void Patrol()
    {
        _animator.SetBool("mo", true); // Ativa a animação de movimento

        // Determina a direção de patrulha com base na posição atual e a posição de destino
        if (!_isReturning)
        {
            if (Vector2.Distance(transform.position, _initialPosition + _moveTarget) < 1f)
            {
                _isReturning = true;
                _currentMoveDirection = (_initialPosition - (Vector2)transform.position).normalized;
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, _initialPosition) < 1f)
            {
                _isReturning = false;
                _currentMoveDirection = (_initialPosition + _moveTarget - (Vector2)transform.position).normalized;
            }
        }

        // Aplica o flip de acordo com a direção do movimento
        if (shouldFlip)
        {
            if (_currentMoveDirection.x > 0)
            {
                transform.localScale = new Vector3(-_originalLocalScaleX, transform.localScale.y, transform.localScale.z);
            }
            else if (_currentMoveDirection.x < 0)
            {
                transform.localScale = new Vector3(_originalLocalScaleX, transform.localScale.y, transform.localScale.z);
            }
        }

        // Move o lanceiro na direção calculada
        transform.position += (Vector3)_currentMoveDirection * moveSpeed * Time.deltaTime;
    }

    // Método para aplicar dano ao Lanceiro
    public void Damage(float damage)
    {
        currentHealth -= damage;
        _animator.SetTrigger("hit");

        if (currentHealth <= 0)
        {
            
            // Instantiate a alma no local do inimigo
            Instantiate(soulPrefab, transform.position, Quaternion.identity);

            // Notifica o GameManager
            GameManager.Instance.AddSoul();
            currentHealth = 0;
            Die(); // Chama a função de morte quando a vida chega a 0
        }
    }

    // Função para a morte do Lanceiro
    private void Die()
    {
        _animator.SetTrigger("death"); // Ativa animação de morte (se tiver)
        Destroy(gameObject, 1f); // Destrói o lanceiro após 1 segundo (tempo de animação)
    }

    private void OnDrawGizmos()
    {
        if (useTransform)
        {
            Debug.DrawLine(transform.position, transform.position + moveDestination.localPosition, Color.yellow);
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + (Vector3)movePosition, Color.red);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
