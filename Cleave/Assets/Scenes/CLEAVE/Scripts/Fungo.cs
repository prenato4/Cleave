using System.Collections;
using UnityEngine;

public class Fungo : MonoBehaviour
{
    public int maxEnergy;
    public int damage;
    public float moveSpeed;
    public bool useTransform;
    public bool shouldFlip;
    
    [SerializeField] private Vector2 movePosition;
    [SerializeField] private Transform moveDestination;
    [SerializeField] private GameObject sporePrefab;
    [SerializeField] private float attackRange = 5f;  // Alcance do ataque
    [SerializeField] private float attackCooldown = 2f;  // Tempo entre os ataques

    private Vector2 _initialPosition;
    private Vector2 _moveTarget;
    private Vector2 _currentMoveDirection;
    private bool _isReturning;
    private float _originalLocalScaleX;
    private int _currentEnergy;
    private Animator _animator;
    private bool _isAlive;
    private Collider2D _collider2D;
    private AudioSource _audioSource;
    private Transform _player;
    private float _lastAttackTime;
    
    public GameObject soulPrefab;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
        _isAlive = true;

        if (shouldFlip) _originalLocalScaleX = transform.localScale.x;

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
        _currentEnergy = maxEnergy;
        _player = GameObject.FindGameObjectWithTag("Player").transform; // Assumindo que o player tem a tag "Player"
    }

    void Update()
    {
        if (!_isAlive) return;

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        _animator.SetBool("mo", true); // Ativa a animação de movimento
        
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

        if (shouldFlip)
        {
            if (_isReturning)
                transform.localScale = new Vector3(-_originalLocalScaleX, transform.localScale.y, transform.localScale.z);
            else
                transform.localScale = new Vector3(_originalLocalScaleX, transform.localScale.y, transform.localScale.z);
        }

        transform.position += (Vector3)_currentMoveDirection * moveSpeed * Time.deltaTime;
    }

    private void AttackPlayer()
    {
        
        _animator.SetBool("mo", false); // Desativa a animação de movimento
        _animator.SetTrigger("at"); 
        
        if (Time.time - _lastAttackTime >= attackCooldown)
        {
            _lastAttackTime = Time.time;

            // Instancia 6 esporos em direções diferentes
            for (int i = 0; i < 10; i++)
            {
                float angle = i * 60f; // 360° / 6 = 60° entre cada esporo
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
            
                // Instancia o esporo e define sua rotação inicial
                GameObject spore = Instantiate(sporePrefab, transform.position, Quaternion.identity);
                spore.transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            //_audioSource.Play(); // Opcional: toca o som do ataque
        }
    }

    public void Damage(int damage)
    {
        _currentEnergy -= damage;

        if (_currentEnergy <= 0)
        {
            _currentEnergy = 0;
            _isAlive = false;
            _collider2D.enabled = false;
            // Instantiate a alma no local do inimigo
            Instantiate(soulPrefab, transform.position, Quaternion.identity);

            // Notifica o GameManager
            GameManager.Instance.AddSoul();
            
            Destroy(gameObject); // Destrói a fada quando ela morre
        }

        if (_currentEnergy > maxEnergy) _currentEnergy = maxEnergy;
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
        Gizmos.DrawWireSphere(transform.position, attackRange); // Mostra o alcance do ataque
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // other.gameObject.GetComponent<IDamageable>().TakeEnergy(damage);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // other.gameObject.GetComponent<IDamageable>().TakeEnergy(damage);
        }
    }
}
