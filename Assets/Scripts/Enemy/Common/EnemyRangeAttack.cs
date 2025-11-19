using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangeAttack : MonoBehaviour
{
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private EnemyConfig _initializedStats;
    [SerializeField] private Animator animator;

    public bool shouldShoot = false;
    
    private float _attackCooldown;
    private float _projectileSpeed;
    private float _projectileLifetime;
    private float _damage;
    public float _range;
    private float _prepareTime;

    private NavMeshAgent agent;
    private Transform _playerTransform;
    private Coroutine _shootingCoroutine;
    private bool _isShooting = false;
    private bool _isPreparing = false;
    RaycastHit hit;

    private bool isPaused;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isPaused) return;

        if (_playerTransform == null) return;

        animator.SetBool("IsPreparing", _isPreparing);
        animator.SetBool("IsShooting", _isShooting);

        float distance = Vector3.Distance(transform.position, _playerTransform.position);

        Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;

        bool seeingPlayer = Physics.Raycast(transform.position, directionToPlayer, out hit, _range, LayerMask.GetMask("Player","Wall"));

        shouldShoot = false;

        if (seeingPlayer)
        {
            shouldShoot = hit.collider.gameObject.layer == LayerMask.NameToLayer("Player");
        }
        else
        {
            shouldShoot = false;
            _isPreparing = false;
        }

        if (shouldShoot && !_isShooting)
        {
            StartShooting();
        }
        else if (!shouldShoot && _isShooting)
        {
            StopShooting();
        }
        if (shouldShoot)
        {
            Vector3 target = new Vector3(_playerTransform.position.x,transform.position.y, _playerTransform.position.z);
            transform.LookAt(target);
        }
    }

    private void OnEnable()
    {
        _attackCooldown = _initializedStats._cooldown;
        _projectileSpeed = _initializedStats._projectileSpeed;
        _range = _initializedStats._range;
        _projectileLifetime = _initializedStats._projectileLifetime;
        _damage = _initializedStats._damage;
        _prepareTime = _initializedStats._prepareTime;
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
        agent.speed = 0;
    }

    private void StopShooting()
    {
        if (_shootingCoroutine != null)
            StopCoroutine(_shootingCoroutine);

        _shootingCoroutine = null;
        _isShooting = false;
        agent.speed = _initializedStats._moveSpeed;
    }

    private IEnumerator ShootingRoutine()
    {
        _isPreparing = true;
        yield return new WaitForSeconds(_prepareTime); 
        _isPreparing = false;

        while (true)
        {
            if (_playerTransform != null)
            {
                ShootAtPlayer();
            }
            yield return new WaitForSeconds(_attackCooldown);
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
        direction.y = 0;
        bulletObj.transform.rotation = Quaternion.LookRotation(direction);

        BulletEnemy bulletController = bulletObj.GetComponent<BulletEnemy>();
        if (bulletController != null)
        {
            bulletController.Initialize(direction, _projectileSpeed, _projectileLifetime, _damage);
        }
        else
        {
            BulletEnemyPool.Instance.ReturnBulletEnemy(bulletObj);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);

        if (_playerTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, _playerTransform.position);
        }
    }
}