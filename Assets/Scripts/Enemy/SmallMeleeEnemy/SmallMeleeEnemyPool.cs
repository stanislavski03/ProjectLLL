using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 2;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy(Vector3 position)
    {

        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.SetActive(true);
            enemy.transform.position = new Vector3(position.x, 0.5f, position.z);
            return enemy;
        }
        else
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.transform.position = new Vector3(position.x, 0.5f, position.z);
            return enemy;
        }
    }

    public void GetEnemyBackToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
