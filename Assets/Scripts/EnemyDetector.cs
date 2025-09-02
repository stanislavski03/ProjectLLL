using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

public class EnemyDetector : MonoBehaviour
{
    public Camera mainCamera;
    [SerializeField] private float _rotationSpeed = 20f;


    private Enemy[] visibleEnemies;


    private void Update()
    {
        FindVisibleEnemies();

        LookAtEnemy(GetClosestEnemy());
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

    public Enemy GetClosestEnemy()
    {
        if (GetVisibleEnemies().Length != 0)
        {
            float minDistance = Mathf.Infinity;
            Enemy[] enemies = GetVisibleEnemies();
            Enemy closestEnemy = null;
            foreach (Enemy enemy in enemies)
            {
                if (enemy == null) continue;

                Vector3 enemyPosition = enemy.transform.position;
                float distance = Vector3.Distance(transform.position, enemyPosition);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }
        return null;

    }



    private void LookAtEnemy(Enemy closestEnemy)
    {
        if (closestEnemy == null) return;

        Vector3 direction = closestEnemy.transform.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                _rotationSpeed * Time.deltaTime);
        }
    }
}