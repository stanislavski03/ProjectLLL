using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

public class EnemyDetector : MonoBehaviour
{
    public Camera mainCamera;


    private Enemy[] visibleEnemies;
    

    private void Update()
    {
        FindVisibleEnemies();

        LookAtEnemy();
    }

    private void FindVisibleEnemies()
    {

        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        List<Enemy> visibleEnemiesList = new List<Enemy>();

        foreach (Enemy enemy in allEnemies)
        {
            if (IsInCameraView(enemy.transform.position))
            {
                visibleEnemiesList.Add(enemy);
            }
        }

        visibleEnemies = visibleEnemiesList.ToArray();

        Debug.Log($"Видимых врагов: {visibleEnemies.Length}");
    }

    private bool IsInCameraView(Vector3 worldPosition)
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(worldPosition);

        bool inCameraView = viewportPoint.z > 0 &&
                           viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                           viewportPoint.y >= 0 && viewportPoint.y <= 1;

        return inCameraView;
    }

    public Enemy[] GetVisibleEnemies()
    {
        return visibleEnemies;
    }

    private void LookAtEnemy()
    {
        float minDistance = 9999999f;//ЭТО НАДО ПЕРЕДЕЛАТЬ!!!
        Enemy[] enemies = GetVisibleEnemies();
        foreach (Enemy enemy in enemies)
        {
            
            Vector3 enemyPosition = enemy.GetComponent<Transform>().position;
            float distance = Vector3.Distance(transform.position, enemyPosition);
            if (minDistance < distance)
                minDistance = distance;
        }
        Debug.Log(minDistance);
    }
}