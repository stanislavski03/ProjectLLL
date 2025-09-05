using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyPool : MonoBehaviour
{
    public static BulletEnemyPool Instance;
    
    [SerializeField] private GameObject bulletEnemyPrefab;
    [SerializeField] private int poolSize = 2;
    
    private Queue<GameObject> bulletEnemyPool = new Queue<GameObject>();
    
    private void Awake()
    {
        Instance = this;
        InitializePool();
    }
    
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bulletEnemy = Instantiate(bulletEnemyPrefab, transform);
            bulletEnemy.SetActive(false);
            bulletEnemyPool.Enqueue(bulletEnemy);
        }
    }
    
    public GameObject GetBulletEnemy()
    {
        if (bulletEnemyPool.Count > 0)
        {
            GameObject bulletEnemy = bulletEnemyPool.Dequeue();
            bulletEnemy.SetActive(true);
            return bulletEnemy;
        }
        else
        {
            GameObject bulletEnemy = Instantiate(bulletEnemyPrefab);
            return bulletEnemy;
        }
    }
    
    public void ReturnBulletEnemy(GameObject bulletEnemy)
    {
        bulletEnemy.SetActive(false);
        bulletEnemyPool.Enqueue(bulletEnemy);
    }
}
