using System.Collections.Generic;
using UnityEngine;

public class ExplosionPool : MonoBehaviour
{
    [SerializeField] private MagicStaffDataSO defaultExplosionData;
    
    private Queue<GameObject> explosionPool = new Queue<GameObject>();
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
        
        for (int i = 0; i < 5; i++)
        {
            CreateNewExplosion();
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
        
        if (explosionPool.Count > 0)
        {
            GameObject explosion = explosionPool.Dequeue();
            if (explosion != null)
            {
                explosion.SetActive(true);
                return explosion;
            }
        }
        
        GameObject newExplosion = CreateNewExplosion();
        newExplosion.SetActive(true);
        return newExplosion;
    }
    
    public void ReturnExplosion(GameObject explosion)
    {
        if (explosion == null) return;
        
        Explosion explosionComponent = explosion.GetComponent<Explosion>();
        if (explosionComponent != null)
        {
        }
        
        explosion.SetActive(false);
        explosion.transform.SetParent(transform);
        explosion.transform.position = Vector3.zero;
        explosion.transform.rotation = Quaternion.identity;
        explosion.transform.localScale = Vector3.one;
        
        explosionPool.Enqueue(explosion);
    }
    
    private void CleanPool()
    {
        Queue<GameObject> cleanPool = new Queue<GameObject>();
        
        foreach (var explosion in explosionPool)
        {
            if (explosion != null)
            {
                cleanPool.Enqueue(explosion);
            }
        }
        
        explosionPool = cleanPool;
    }
}