using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyPool : MonoBehaviour
{
    public static BulletEnemyPool Instance;
    
    [SerializeField] private GameObject _bulletEnemyPrefab;
    [SerializeField] private int _initialPoolSize = 5;
    [SerializeField] private int _expandAmount = 3;
    
    private Queue<GameObject> _bulletPool = new Queue<GameObject>();
    private List<GameObject> _allBullets = new List<GameObject>();

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
        ExpandPool(_initialPoolSize);
    }

    private void ExpandPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateNewBullet();
        }
    }

    private GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate(_bulletEnemyPrefab, transform);
        bullet.SetActive(false);
        _bulletPool.Enqueue(bullet);
        _allBullets.Add(bullet);
        return bullet;
    }

    public GameObject GetBulletEnemy()
    {
        CleanPool();
        
        if (_bulletPool.Count == 0)
        {
            ExpandPool(_expandAmount);
        }
        
        while (_bulletPool.Count > 0)
        {
            GameObject bullet = _bulletPool.Dequeue();
            if (bullet != null && !bullet.Equals(null))
            {
                bullet.SetActive(true);
                return bullet;
            }
        }
        
        return CreateNewBullet();
    }

    public void ReturnBulletEnemy(GameObject bulletEnemy)
    {
        if (bulletEnemy == null || bulletEnemy.Equals(null)) return;
        
        bulletEnemy.SetActive(false);
        bulletEnemy.transform.SetParent(transform);
        bulletEnemy.transform.position = Vector3.zero;
        bulletEnemy.transform.rotation = Quaternion.identity;
        
        if (!_bulletPool.Contains(bulletEnemy))
        {
            _bulletPool.Enqueue(bulletEnemy);
        }
    }

    private void CleanPool()
    {
        List<GameObject> validBullets = new List<GameObject>();
        
        while (_bulletPool.Count > 0)
        {
            GameObject bullet = _bulletPool.Dequeue();
            if (bullet != null && !bullet.Equals(null))
            {
                validBullets.Add(bullet);
            }
        }
        
        foreach (GameObject bullet in validBullets)
        {
            _bulletPool.Enqueue(bullet);
        }
    }

    public int GetActiveCount()
    {
        int count = 0;
        foreach (var bullet in _allBullets)
        {
            if (bullet != null && !bullet.Equals(null) && bullet.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }
    
    public void ReinitializePool()
    {
        foreach (var bullet in _allBullets)
        {
            if (bullet != null && !bullet.Equals(null))
            {
                Destroy(bullet);
            }
        }
        
        _bulletPool.Clear();
        _allBullets.Clear();
        ExpandPool(_initialPoolSize);
    }
}