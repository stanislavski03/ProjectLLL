using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Camera _spawnCamera;

    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
        _spawnCamera = Camera.main;
    }

    public Vector3 GetRandomEdgePoint()
    {
        // Получаем размеры экрана в мировых координатах
        float _cameraHeight = _spawnCamera.orthographicSize * 2f;
        float _cameraWidth = _cameraHeight * _spawnCamera.aspect;

        int _edge = Random.Range(0, 4);

        Vector3 randomPoint = Vector3.zero;

        switch (_edge)
        {
            case 0: // Верхняя граница
                randomPoint = new Vector3(_playerTransform.position.x + Random.Range(-_cameraWidth / 2f, _cameraWidth / 2f), _playerTransform.position.y , _playerTransform.position.z + (_cameraHeight / 2f) +3);
                break;
            case 1: // Нижняя граница
                randomPoint = new Vector3(_playerTransform.position.x + Random.Range(-_cameraWidth / 2f, _cameraWidth / 2f), _playerTransform.position.y, _playerTransform.position.z + (-_cameraHeight / 2f) -3);
                break;
            case 2: // Левая граница
                randomPoint = new Vector3(_playerTransform.position.x + (-_cameraWidth / 2f) - 3, _playerTransform.position.y, _playerTransform.position.z + Random.Range(-_cameraHeight / 2f, _cameraHeight / 2f));
                break;
            case 3: // Правая граница
                randomPoint = new Vector3(_playerTransform.position.x + (_cameraWidth / 2f) + 3, _playerTransform.position.y, _playerTransform.position.z + Random.Range(-_cameraHeight / 2f, _cameraHeight / 2f));
                break;
        }

        return randomPoint;
    }

    void Update()
    {
        // Пример: при нажатии кнопки "пробел", генерируем точку
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 _newPosition = GetRandomEdgePoint();
            GameObject enemyObj = EnemyPool.Instance.GetEnemy(_newPosition);
        }
}

}
