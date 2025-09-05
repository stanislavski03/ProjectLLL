using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingDmg : MonoBehaviour
{
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _bulletSpawnCooldown = 1f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _shootSpeed = 5f;
    [SerializeField] private float _damageCooldown = 1f;

    private Transform _playerTransform;
    private float _cooldownTimer = 0;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void Start()
    {
        InvokeRepeating(nameof(ShootInPlayer), 1f, _bulletSpawnCooldown);
    }

    private void OnEnable()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnCollisionStay(Collision collision)
    {
        PlayerCheckAndDamage(collision);
    }

    private void PlayerCheckAndDamage(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHP player) && _cooldownTimer <= 0)
        {
            player.Damage(_damage);
            StartCoroutine(DamageCooldown());
        }
    }
    private IEnumerator DamageCooldown()
    {
        for (_cooldownTimer = _damageCooldown; _cooldownTimer > 0; _cooldownTimer -= 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }

    }

    private void ShootInPlayer()
    {
        GameObject bulletObj = BulletPool.Instance.GetBullet();
        bulletObj.transform.position = _bulletSpawn.position;
        bulletObj.transform.rotation = _bulletSpawn.rotation;

        Bullet bulletController = bulletObj.GetComponent<Bullet>();
        if (bulletController != null)
        {
            bulletController.ResetBullet(_playerTransform, _shootSpeed);
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
        //ситуативно, если надо чтобы враги не наносили урон сразу как отожмётся пауза
        if (enabled)
        {
            StartCoroutine(DamageCooldown());
            Debug.Log("StartCoroutine");
        }
        else
        {
            StopCoroutine(DamageCooldown());
            Debug.Log("StopCoroutine");
        }
    }
}
