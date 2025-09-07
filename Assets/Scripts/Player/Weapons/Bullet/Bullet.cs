using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;
    [SerializeField] private LayerMask _collisionLayers = Physics.DefaultRaycastLayers;
    
    private Transform _target;
    private float _speed;
    private float _lifetime;
    private int _damageType;
    private bool _hasTarget;
    private Vector3 _lastDirection;
    
    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(ReturnToPool), _lifetime);
    }
    
    private void OnDisable()
    {
        CancelInvoke();
        _target = null;
        _hasTarget = false;
    }
    
    private void Update()
    {
        if (_hasTarget && _target != null && _target.gameObject.activeInHierarchy)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;
            
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
            
            _lastDirection = direction;
            
            CheckCollisionWithRaycast();
        }
        else
        {
            transform.position += _lastDirection * _speed * Time.deltaTime;
            CheckCollisionWithRaycast();
        }
    }
    
    private void CheckCollisionWithRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, _lastDirection, out hit, _speed * Time.deltaTime + 0.1f, _collisionLayers))
        {
            HandleCollision(hit.collider);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.collider);
    }
    
    private void HandleCollision(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            if (collider.TryGetComponent(out EnemyHP enemy))
            {
                enemy.Damage(_damage, _damageType);
            }
            ReturnToPool();
        }
    }
    
    public void ResetBullet(Transform newTarget, float newSpeed, float newLifetime, int damageType)
    {
        _target = newTarget;
        _speed = newSpeed;
        _lifetime = newLifetime;
        _damageType = damageType;
        _hasTarget = newTarget != null;
        
        if (_hasTarget)
        {
            _lastDirection = (_target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(_lastDirection);
        }
        else
        {
            _lastDirection = transform.forward;
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _lastDirection * 2f);
    }
}