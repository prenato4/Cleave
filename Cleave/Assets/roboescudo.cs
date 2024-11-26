using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roboescudo : MonoBehaviour
{
     public int maxEnergy;
    public int damage;
    public float moveSpeed;
    public bool useTransform;
    public bool shouldFlip;
    
    [SerializeField] private Vector2 movePosition;
    [SerializeField] private Transform moveDestination;
    
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

        // Salva o valor absoluto da escala inicial no eixo X
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
        _currentEnergy = maxEnergy;

        // Localiza o player na cena
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!_isAlive) return;

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        
            FacePlayer(); // Faz o fungo olhar para o player
            
        
            MovePlatform();
        
    }
    
    private void FacePlayer()
    {
        if (_player == null) return;

        // Determina a direção do player
        float direction = _player.position.x - transform.position.x;

        // Ajusta a escala do fungo para encarar o player
        if (direction < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(_originalLocalScaleX), transform.localScale.y, transform.localScale.z);
        }
        else if (direction > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(_originalLocalScaleX), transform.localScale.y, transform.localScale.z);
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

    public void Damage(int damage)
    {
        _currentEnergy -= damage;
        _animator.SetTrigger("hit2");

        if (_currentEnergy <= 0)
        {
            _currentEnergy = 0;
            _isAlive = false;
            _collider2D.enabled = false;
            _animator.SetTrigger("death2");
            // Instantiate a alma no local do inimigo
            Instantiate(soulPrefab, transform.position, Quaternion.identity);

            // Notifica o GameManager
            GameManager.Instance.AddSoul();
            
            Destroy(gameObject, 2f); // Destrói a fada quando ela morre
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
