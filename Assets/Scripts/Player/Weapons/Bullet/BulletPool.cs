using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private BulletShooterDataSO defaultBulletData;
    
    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    [SerializeField] private GameObject bulletPrefab;

    public static BulletPool Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
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
        
        if (bulletPool.Count == 0)
        {
            // Если пул пуст, расширяем его
            CreateNewBullet();
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
        return newBullet;
    }
    
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        

            bulletPool.Enqueue(bullet);
    }
    
    
    // Метод для полной переинициализации пула
   
}