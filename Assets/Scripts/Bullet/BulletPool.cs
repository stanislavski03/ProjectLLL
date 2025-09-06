using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _initialPoolSize = 2;
    
    private Queue<GameObject> _bulletPool = new Queue<GameObject>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializePool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            CreateNewBullet();
        }
    }
    
    private GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab, transform);
        bullet.SetActive(false);
        _bulletPool.Enqueue(bullet);
        return bullet;
    }
    
    public GameObject GetBullet()
    {
        CleanPool();
        
        if (_bulletPool.Count > 0)
        {
            GameObject bullet = _bulletPool.Dequeue();
            if (bullet != null)
            {
                return bullet;
            }
        }
        
        return CreateNewBullet();
    }
    
    public void ReturnBullet(GameObject bullet)
    {
        if (bullet == null) return;
        
        bullet.SetActive(false);
        bullet.transform.SetParent(transform);
        bullet.transform.position = Vector3.zero;
        bullet.transform.rotation = Quaternion.identity;
        
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            // Можно добавить сброс состояния если нужно
        }
        
        _bulletPool.Enqueue(bullet);
    }
    
    private void CleanPool()
    {
        int initialCount = _bulletPool.Count;
        Queue<GameObject> cleanPool = new Queue<GameObject>();
        
        foreach (var bullet in _bulletPool)
        {
            if (bullet != null)
            {
                cleanPool.Enqueue(bullet);
            }
        }
        
        _bulletPool = cleanPool;
    }
    
    public int GetActiveBulletsCount()
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }
    
    public int GetPoolSize()
    {
        return _bulletPool.Count;
    }
}