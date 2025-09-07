using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 3f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private LayerMask _collisionLayers = Physics.DefaultRaycastLayers;

    private Vector3 _direction;
    private float _currentLifetime;
    private PlayerHP _playerHP;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerHP = player.GetComponent<PlayerHP>();
        }
        
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnEnable()
    {
        _currentLifetime = _lifetime;
        CancelInvoke();
        Invoke(nameof(ReturnToPool), _lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        if (!enabled) return;
        
        _currentLifetime -= Time.deltaTime;
        if (_currentLifetime <= 0)
        {
            ReturnToPool();
            return;
        }
        
        transform.Translate(Vector3.forward * _speed * Time.deltaTime, Space.Self);
        CheckCollision();
    }

    private void CheckCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 
            _speed * Time.deltaTime + 0.1f, _collisionLayers))
        {
            HandleCollision(hit.collider);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }

    private void HandleCollision(Collider collider)
    {
        if (collider.CompareTag("Player") && _playerHP != null)
        {
            _playerHP.Damage(_damage);
            ReturnToPool();
        }
        else if (collider.CompareTag("Environment") || collider.CompareTag("Wall") || collider.CompareTag("Bullet"))
        {
            ReturnToPool();
        }
    }

    public void Initialize(Vector3 direction, float speed)
    {
        _direction = direction.normalized;
        _speed = speed;
        transform.rotation = Quaternion.LookRotation(_direction);
        
        CancelInvoke();
        Invoke(nameof(ReturnToPool), _lifetime);
    }

    private void ReturnToPool()
    {
        if (BulletEnemyPool.Instance != null)
        {
            BulletEnemyPool.Instance.ReturnBulletEnemy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
}