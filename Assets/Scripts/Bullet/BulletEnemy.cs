using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    [SerializeField] private float _damage = 10f;

    private Transform _playerTransform;
    private PlayerHP _playerHP;
    private Vector3 _direction;
    private bool _isInitialized = false;

    private void Awake()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
        _playerHP = _playerTransform.GetComponent<PlayerHP>();

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnEnable()
    {
        _isInitialized = false;
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        if (!_isInitialized && _playerTransform != null)
        {
            _direction = (_playerTransform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(_direction);
            _isInitialized = true;
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out EnemyHP enemy))
        {
            enemy.Damage(10);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerHP.Damage(_damage);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        BulletEnemyPool.Instance.ReturnBulletEnemy(gameObject);
    }

    public void ResetBulletEnemy(float newSpeed)
    {
        speed = newSpeed;
        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifetime);
        _isInitialized = false;
    }

    public void SetDirection(Vector3 direction, float bulletSpeed)
    {
        _direction = direction.normalized;
        speed = bulletSpeed;
        transform.rotation = Quaternion.LookRotation(_direction);
        _isInitialized = true;

        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifetime);
    }
    
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
