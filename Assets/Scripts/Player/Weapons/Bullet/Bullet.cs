using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    //[SerializeField] private LayerMask collisionLayers = Physics.DefaultRaycastLayers;

    private Transform target;
    private float currentSpeed;
    private float currentLifetime;
    private int currentDamageType;
    private float currentDamage;
    private bool hasTarget;
    private Vector3 lastDirection;
    private Weapon sourceWeapon;

    private void Update()
    {
        if (hasTarget && target != null && target.gameObject.activeInHierarchy)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            direction = new Vector3(direction.x, 0, direction.z);
            transform.position += direction * currentSpeed * Time.deltaTime;
            lastDirection = direction;
        }
        else
        {
            target = null;
            transform.position += lastDirection * currentSpeed * Time.deltaTime;
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

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out EnemyHP enemy))
            {

                float actualDamage = sourceWeapon != null ? sourceWeapon.GetDamage() : currentDamage;
                enemy.Damage(actualDamage);

            }
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        // Отменяем все вызовы методов
        CancelInvoke();

        if (BulletPool.Instance != null && gameObject != null && gameObject.activeInHierarchy)
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }

    private void ResetBullet()
    {
        target = null;
        hasTarget = false;
        sourceWeapon = null;
        lastDirection = Vector3.zero;
    }
}