using UnityEngine;
using System.Collections.Generic;

public class EnemyDetector : MonoBehaviour, IGameplaySystem
{
    
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private GameObject _menuCamera;
    [SerializeField] private float _updateInterval = 0.2f;

    private Enemy[] _visibleEnemies;
    private float _updateTimer;
    private bool isPaused;

    public Camera mainCamera;

    private void Start()
    {
        // Автоматически регистрируемся через интерфейс
        // GameStateManager найдет нас при старте
    }

    private void Update()
    {
        if (isPaused) return;
        
        _updateTimer -= Time.deltaTime;
        
        if (_updateTimer <= 0f)
        {
            FindVisibleEnemies();
            _updateTimer = _updateInterval;
        }

        LookAtEnemy(GetClosestEnemy());
    }

    private void FindVisibleEnemies()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        List<Enemy> visibleEnemiesList = new List<Enemy>();

        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy && IsInCameraView(enemy.transform.position))
            {
                visibleEnemiesList.Add(enemy);
            }
        }

        _visibleEnemies = visibleEnemiesList.ToArray();
    }

    private bool IsInCameraView(Vector3 worldPosition)
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null) return false;

        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(worldPosition);

        return viewportPoint.z > 0 &&
               viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }

    public Enemy[] GetVisibleEnemies()
    {
        return _visibleEnemies ?? new Enemy[0];
    }

    public Enemy GetClosestEnemy()
    {
        Enemy[] enemies = GetVisibleEnemies();
        if (enemies.Length == 0) return null;

        float minDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        
        foreach (Enemy enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }
        
        return closestEnemy;
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
    
    private void LookAtMenuCamera()
    {
        Vector3 lookPosition = new Vector3(_menuCamera.transform.position.x, 1, _menuCamera.transform.position.z);
        if (_menuCamera != null)
        {
            transform.LookAt(lookPosition);
        }
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
        
        if (paused)
        {
            // LookAtMenuCamera();
        }
        else
        {
            FindVisibleEnemies();
        }
    }
}