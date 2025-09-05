using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;
    public float speed = 10f;
    public float lifetime = 3f;

    private Vector3 _direction;

    private void Awake()
    {
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
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
            transform.LookAt(target);
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
            ReturnToPool();
        }

        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    ReturnToPool();
        //}
    }

    private void ReturnToPool()
    {
        BulletPool.Instance.GetBulletBackToPool(gameObject);
    }

    public void ResetBullet(Transform newTarget, float newSpeed)
    {
        target = newTarget;
        speed = newSpeed;
        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifetime);
    }

    //public void SetDirection(Vector3 direction, float bulletSpeed)
    //{
    //    _direction = direction.normalized;
    //    speed = bulletSpeed;

    //    if (_direction != Vector3.zero)
    //    {
    //        transform.rotation = Quaternion.LookRotation(_direction);
    //    }
    //}
    
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
