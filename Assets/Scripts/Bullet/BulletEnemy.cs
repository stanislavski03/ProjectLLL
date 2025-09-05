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

    private void Awake()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
        _playerHP = _playerTransform.GetComponent<PlayerHP>();

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        if (_playerTransform != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _playerTransform.position,
                speed * Time.deltaTime
            );
            transform.LookAt(_playerTransform);
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
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

    public void ResetBulletEnemy(Transform newTarget, float newSpeed)
    {
        _playerTransform = newTarget;
        speed = newSpeed;
        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifetime);
    }

    public void SetDirection(Vector3 direction, float bulletSpeed)
    {
        _direction = direction.normalized;
        speed = bulletSpeed;

        if (_direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_direction);
        }
    }
    
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
