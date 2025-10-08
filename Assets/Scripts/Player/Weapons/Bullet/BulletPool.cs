using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private BulletShooterDataSO defaultBulletData;
    
    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    private GameObject bulletPrefab;

    public static BulletPool Instance { get; private set; }
    
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
        // Используем префаб из ScriptableObject или резервный
        if (defaultBulletData != null && defaultBulletData.bulletPrefab != null)
        {
            bulletPrefab = defaultBulletData.bulletPrefab;
        }
        else
        {
            Debug.LogError("No bullet prefab assigned in BulletPool!");
            return;
        }
        
        for (int i = 0; i < 1; i++) // Динамический размер пула
        {
            CreateNewBullet();
        }
    }
    
    private GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
        return bullet;
    }
    
    public GameObject GetBullet()
    {
        CleanPool();
        
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
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
        
        bulletPool.Enqueue(bullet);
    }
    
    private void CleanPool()
    {
        int initialCount = bulletPool.Count;
        Queue<GameObject> cleanPool = new Queue<GameObject>();
        
        foreach (var bullet in bulletPool)
        {
            if (bullet != null)
            {
                cleanPool.Enqueue(bullet);
            }
        }
        
        bulletPool = cleanPool;
    }
    
    public void UpdateBulletPrefab(GameObject newPrefab)
    {
        if (newPrefab != null)
        {
            bulletPrefab = newPrefab;
            // Можно очистить пул и пересоздать с новым префабом
        }
    }
}