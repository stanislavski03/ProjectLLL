using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 3f;
    [SerializeField] private float _damage = 10f;

    private Vector3 _direction;
    private float _currentLifetime;
    private PlayerHP _playerHP;

    private bool isPaused;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerHP = player.GetComponent<PlayerHP>();
        }
    }

    private void Update()
    {
        if (isPaused) return;

        _currentLifetime -= Time.deltaTime;
        if (_currentLifetime <= 0)
        {
            ReturnToPool();
            return;
        }

        transform.Translate(Vector3.forward * _speed * Time.deltaTime, Space.Self);
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

    public void Initialize(Vector3 direction, float speed, float lifetime, float damage)
    {
        _direction = direction.normalized;
        _speed = speed;
        _lifetime = lifetime;
        _damage = damage;
        transform.rotation = Quaternion.LookRotation(_direction);

        CancelInvoke();
        Invoke(nameof(ReturnToPool), _lifetime);
    }

    private void ReturnToPool()
    {
        CancelInvoke();
        
        if (BulletEnemyPool.Instance != null && gameObject != null && gameObject.activeInHierarchy)
        {
            BulletEnemyPool.Instance.ReturnBulletEnemy(gameObject);
        }
        else if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
}