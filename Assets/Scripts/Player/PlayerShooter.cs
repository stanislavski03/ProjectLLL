using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _shootSpeed;
    [SerializeField] private float _bulletSpawnCooldown = 0.5f;

    private EnemyDetector _enemyDetector;

    private void Awake()
    {
        _enemyDetector = GetComponent<EnemyDetector>();

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void Start()
    {
        InvokeRepeating(nameof(ShootClosestEnemy), 1f, _bulletSpawnCooldown);
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void ShootClosestEnemy()
    {
        Enemy closestEnemy = _enemyDetector.GetClosestEnemy();
        if (closestEnemy != null)
        {
            Shoot(closestEnemy);
        }
    }

    private void Shoot(Enemy enemy)
    {
        GameObject bulletObj = BulletPool.Instance.GetBullet();
        bulletObj.transform.position = _bulletSpawn.position;
        bulletObj.transform.rotation = _bulletSpawn.rotation;

        Bullet bulletController = bulletObj.GetComponent<Bullet>();
        if (bulletController != null)
        {
            bulletController.ResetBullet(enemy.transform, _shootSpeed);
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;

        if (enabled)
        {
            InvokeRepeating(nameof(ShootClosestEnemy), 1f, 0.5f);
        }
        else
        {
            CancelInvoke(nameof(ShootClosestEnemy));
        }

    }
}
