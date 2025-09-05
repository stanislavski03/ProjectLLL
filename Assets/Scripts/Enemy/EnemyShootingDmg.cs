using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingDmg : MonoBehaviour
{
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _bulletSpawnCooldown = 1f;
    [SerializeField] private float _shootSpeed = 5f;

    private Transform _playerTransform;
    private EnemyMovement _enemyMovementController;
    private bool shootFlag = false;

    private void Awake()
    {
        _enemyMovementController = GetComponent<EnemyMovement>();

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) <= _enemyMovementController._playerRange)
        {
            if (shootFlag) return;
            InvokeRepeating(nameof(ShootInPlayer), 1f, _bulletSpawnCooldown);
            shootFlag = true;
        }
        else
        {
            if (!shootFlag) return;
            CancelInvoke(nameof(ShootInPlayer));
            shootFlag = false;
        }
    }

    private void OnEnable()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void ShootInPlayer()
    {
        GameObject bulletObj = BulletEnemyPool.Instance.GetBulletEnemy();
        bulletObj.transform.position = _bulletSpawn.position;
        bulletObj.transform.rotation = _bulletSpawn.rotation;

        BulletEnemy bulletController = bulletObj.GetComponent<BulletEnemy>();
        if (bulletController != null)
        {
            bulletController.ResetBulletEnemy(_shootSpeed);
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;

        if (enabled)
        {
            InvokeRepeating(nameof(ShootInPlayer), 1f, 0.5f);
        }
        else
        {
            CancelInvoke(nameof(ShootInPlayer));
        }

    }
}
