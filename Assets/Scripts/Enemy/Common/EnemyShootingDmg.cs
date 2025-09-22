using System.Collections;
using UnityEngine;

public class EnemyShootingDmg : MonoBehaviour, IGameplaySystem
{
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _bulletSpawnCooldown = 1f;
    [SerializeField] private float _shootSpeed = 5f;
    [SerializeField] private float _playerDetectionRange = 10f;

    private Transform _playerTransform;
    private Coroutine _shootingCoroutine;
    private bool _isShooting = false;

    private bool isPaused;

    private void Update()
    {
        if (isPaused) return;

        if (_playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, _playerTransform.position);
        bool shouldShoot = distance <= _playerDetectionRange;

        if (shouldShoot && !_isShooting)
        {
            StartShooting();
        }
        else if (!shouldShoot && _isShooting)
        {
            StopShooting();
        }
    }

    private void OnEnable()
    {
        _playerTransform = GameObject.FindWithTag("Player")?.transform;
    }

    private void OnDestroy()
    {
        StopShooting();
    }

    private void StartShooting()
    {
        if (_shootingCoroutine != null)
            StopCoroutine(_shootingCoroutine);

        _shootingCoroutine = StartCoroutine(ShootingRoutine());
        _isShooting = true;
    }

    private void StopShooting()
    {
        if (_shootingCoroutine != null)
            StopCoroutine(_shootingCoroutine);

        _shootingCoroutine = null;
        _isShooting = false;
    }

    private IEnumerator ShootingRoutine()
    {
        yield return new WaitForSeconds(1f); // Начальная задержка

        while (true)
        {
            if (_playerTransform != null)
            {
                ShootAtPlayer();
            }
            yield return new WaitForSeconds(_bulletSpawnCooldown);
        }
    }

    private void ShootAtPlayer()
    {
        if (isPaused) return;

        if (_playerTransform == null || BulletEnemyPool.Instance == null) return;

        GameObject bulletObj = BulletEnemyPool.Instance.GetBulletEnemy();
        if (bulletObj == null) return;

        bulletObj.transform.position = _bulletSpawn.position;

        Vector3 direction = (_playerTransform.position - _bulletSpawn.position).normalized;
        bulletObj.transform.rotation = Quaternion.LookRotation(direction);

        BulletEnemy bulletController = bulletObj.GetComponent<BulletEnemy>();
        if (bulletController != null)
        {
            bulletController.Initialize(direction, _shootSpeed);
        }
        else
        {
            BulletEnemyPool.Instance.ReturnBulletEnemy(bulletObj);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _playerDetectionRange);
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;

        if (paused)
        {
            StopShooting();
            enabled = false;
        }
        else
        {
            enabled = true;
        }
    }
}