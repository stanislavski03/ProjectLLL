using System.Collections.Generic;
using UnityEngine;

public class ExplosionPool : MonoBehaviour
{
    [SerializeField] private MagicStaffDataSO defaultExplosionData;
    [SerializeField] private int initialPoolSize = 3;
    [SerializeField] private int expandAmount = 2;
    
    private Queue<GameObject> explosionPool = new Queue<GameObject>();
    private List<GameObject> allExplosions = new List<GameObject>();
    private GameObject explosionPrefab;

    public static ExplosionPool Instance { get; private set; }
    
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
        if (defaultExplosionData != null && defaultExplosionData.explosionPrefab != null)
        {
            explosionPrefab = defaultExplosionData.explosionPrefab;
        }
        else
        {
            Debug.LogError("No explosion prefab assigned in ExplosionPool!");
            return;
        }
        
        ExpandPool(initialPoolSize);
    }
    
    private void ExpandPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject explosion = CreateNewExplosion();
            allExplosions.Add(explosion);
        }
    }
    
    private GameObject CreateNewExplosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform);
        explosion.SetActive(false);
        explosionPool.Enqueue(explosion);
        return explosion;
    }
    
    public GameObject GetExplosion()
    {
        CleanPool();
        
        if (explosionPool.Count == 0)
        {
            ExpandPool(expandAmount);
        }
        
        if (explosionPool.Count > 0)
        {
            GameObject explosion = explosionPool.Dequeue();
            if (explosion != null && !explosion.Equals(null))
            {
                explosion.SetActive(true);
                return explosion;
            }
        }
        
        GameObject newExplosion = CreateNewExplosion();
        allExplosions.Add(newExplosion);
        newExplosion.SetActive(true);
        return newExplosion;
    }
    
    public void ReturnExplosion(GameObject explosion)
    {
        if (explosion == null || explosion.Equals(null)) return;
        
        explosion.SetActive(false);
        explosion.transform.SetParent(transform);
        explosion.transform.position = Vector3.zero;
        explosion.transform.rotation = Quaternion.identity;
        explosion.transform.localScale = Vector3.one;
        
        if (!explosionPool.Contains(explosion))
        {
            explosionPool.Enqueue(explosion);
        }
    }
    
    private void CleanPool()
    {
        List<GameObject> validExplosions = new List<GameObject>();
        
        while (explosionPool.Count > 0)
        {
            GameObject explosion = explosionPool.Dequeue();
            if (explosion != null && !explosion.Equals(null))
            {
                validExplosions.Add(explosion);
            }
        }
        
        foreach (GameObject explosion in validExplosions)
        {
            explosionPool.Enqueue(explosion);
        }
    }
    
    public void ReinitializePool()
    {
        foreach (var explosion in allExplosions)
        {
            if (explosion != null && !explosion.Equals(null))
            {
                Destroy(explosion);
            }
        }
        
        explosionPool.Clear();
        allExplosions.Clear();
        ExpandPool(initialPoolSize);
    }
}