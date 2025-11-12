using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private BulletShooterDataSO defaultBulletData;
    [SerializeField] private int initialPoolSize = 3;
    [SerializeField] private int expandAmount = 5;
    
    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    private List<GameObject> allBullets = new List<GameObject>(); // Для отслеживания всех созданных пуль
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
        if (defaultBulletData != null && defaultBulletData.bulletPrefab != null)
        {
            bulletPrefab = defaultBulletData.bulletPrefab;
        }
        else
        {
            Debug.LogError("No bullet prefab assigned in BulletPool!");
            return;
        }
        
        // Создаем начальный пул
        ExpandPool(initialPoolSize);
    }
    
    private void ExpandPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject bullet = CreateNewBullet();
            allBullets.Add(bullet);
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
        CleanPool(); // Очищаем пул от уничтоженных пуль ПЕРЕД получением
        
        if (bulletPool.Count == 0)
        {
            // Если пул пуст, расширяем его
            ExpandPool(expandAmount);
            Debug.Log($"Pool expanded. Total bullets: {allBullets.Count}");
        }
        
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            
            // Двойная проверка (на всякий случай)
            if (bullet != null)
            {
                return bullet;
            }
        }
        
        // Если всё еще не получили пулю, создаем новую
        GameObject newBullet = CreateNewBullet();
        allBullets.Add(newBullet);
        return newBullet;
    }
    
    public void ReturnBullet(GameObject bullet)
    {
        if (bullet == null) return;
        
        // Проверяем, не уничтожена ли пуля
        if (bullet.Equals(null)) return;
        
        bullet.SetActive(false);
        bullet.transform.SetParent(transform);
        bullet.transform.position = Vector3.zero;
        bullet.transform.rotation = Quaternion.identity;
        
        // Проверяем, нет ли уже этой пули в пуле (на всякий случай)
        if (!bulletPool.Contains(bullet))
        {
            bulletPool.Enqueue(bullet);
        }
    }
    
    // Улучшенная очистка пула
    private void CleanPool()
    {
        int initialCount = bulletPool.Count;
        
        // Создаем временный список для валидных пуль
        List<GameObject> validBullets = new List<GameObject>();
        
        while (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            if (bullet != null && !bullet.Equals(null))
            {
                validBullets.Add(bullet);
            }
        }
        
        // Возвращаем только валидные пули обратно в очередь
        foreach (GameObject bullet in validBullets)
        {
            bulletPool.Enqueue(bullet);
        }
        
        // Логируем только если были удалены пули
        if (initialCount != bulletPool.Count)
        {
            Debug.Log($"Pool cleaned: {initialCount} -> {bulletPool.Count}. Total created: {allBullets.Count}");
        }
    }
    
    // Метод для полной переинициализации пула
    public void ReinitializePool()
    {
        // Очищаем текущие пули
        foreach (var bullet in allBullets)
        {
            if (bullet != null && !bullet.Equals(null))
            {
                Destroy(bullet);
            }
        }
        
        bulletPool.Clear();
        allBullets.Clear();
        
        // Создаем новый пул
        ExpandPool(initialPoolSize);
    }
}