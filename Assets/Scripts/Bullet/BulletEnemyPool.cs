using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyPool : MonoBehaviour
{
    public static BulletEnemyPool Instance;
    
    [SerializeField] private GameObject _bulletEnemyPrefab;
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
        GameObject bullet = Instantiate(_bulletEnemyPrefab, transform);
        bullet.SetActive(false);
        _bulletPool.Enqueue(bullet);
        return bullet;
    }

    public GameObject GetBulletEnemy()
    {
        CleanPool();
        
        // Ищем первую неактивную пулю в пуле
        foreach (var bullet in _bulletPool)
        {
            if (bullet != null && !bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }
        
        // Если все пули активны - создаем новую
        return CreateNewBullet();
    }

    public void ReturnBulletEnemy(GameObject bulletEnemy)
    {
        if (bulletEnemy == null) return;
        
        bulletEnemy.SetActive(false);
        bulletEnemy.transform.SetParent(transform);
        bulletEnemy.transform.position = Vector3.zero;
        bulletEnemy.transform.rotation = Quaternion.identity;
        
        _bulletPool.Enqueue(bulletEnemy);
    }

    private void CleanPool()
    {
        // Удаляем уничтоженные объекты
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

    public int GetActiveCount()
    {
        int count = 0;
        foreach (var bullet in _bulletPool)
        {
            if (bullet != null && bullet.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }
}