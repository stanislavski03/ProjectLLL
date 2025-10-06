using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private LayerMask collisionLayers = Physics.DefaultRaycastLayers;
    [SerializeField] private GameObject hitEffect;
    
    private Transform target;
    private float currentSpeed;
    private float currentLifetime;
    private int currentDamageType;
    private float currentDamage;
    private bool hasTarget;
    private Vector3 lastDirection;

    // Ссылка на источник урона (для возможных будущих улучшений)
    private Weapon sourceWeapon;

    private void Update()
    {
        if (hasTarget && target != null && target.gameObject.activeInHierarchy)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * currentSpeed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            lastDirection = direction;
            CheckCollisionWithRaycast();
        }
        else
        {
            // Двигаемся в последнем направлении если цель потеряна
            transform.position += lastDirection * currentSpeed * Time.deltaTime;
            CheckCollisionWithRaycast();
        }
    }

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(ReturnToPool), currentLifetime);
    }

    private void OnDisable()
    {
        CancelInvoke();
        ResetBullet();
    }


    public void UpdateStats(float damage, float speed, int damageType)
    {
        currentDamage = damage;
        currentSpeed = speed;
        currentDamageType = damageType;
    }

    public void InitializeBullet(Transform newTarget, float newSpeed, float newLifetime, int newDamageType, float newDamage, Weapon source = null)
    {
        target = newTarget;
        currentSpeed = newSpeed;
        currentLifetime = newLifetime;
        currentDamageType = newDamageType;
        currentDamage = newDamage;
        sourceWeapon = source;
        hasTarget = newTarget != null;

        if (hasTarget)
        {
            lastDirection = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(lastDirection);
        }
        else
        {
            lastDirection = transform.forward;
        }
    }

    private void CheckCollisionWithRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, lastDirection, out hit, currentSpeed * Time.deltaTime + 0.1f, collisionLayers))
        {
            HandleCollision(hit.collider, hit.point);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other, transform.position);
    }

    private void HandleCollision(Collider collider, Vector3 hitPoint)
    {
        if (collider.CompareTag("Enemy"))
        {
            if (collider.TryGetComponent(out EnemyHP enemy))
            {
                enemy.Damage(currentDamage, currentDamageType);
                PlayHitEffect(hitPoint);
            }
            ReturnToPool();
        }
        else if (collider.CompareTag("Environment")) // Столкновение с окружением
        {
            PlayHitEffect(hitPoint);
            ReturnToPool();
        }
    }

    private void PlayHitEffect(Vector3 position)
    {
        if (hitEffect != null)
        {
            // Здесь можно создать систему пула для эффектов
            Instantiate(hitEffect, position, Quaternion.identity);
        }
    }

    private void ReturnToPool()
    {
        if (BulletPool.Instance != null)
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ResetBullet()
    {
        target = null;
        hasTarget = false;
        sourceWeapon = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + lastDirection * 2f);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}