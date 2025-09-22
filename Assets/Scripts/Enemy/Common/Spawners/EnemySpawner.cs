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

    void Update()
    {
        // ������: ��� ������� ������ "������", ���������� �����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 _newPosition = GetRandomEdgePoint();
            GameObject enemyObj = EnemyPool.Instance.GetEnemy(_newPosition);
        }
    }

    public Vector3 GetRandomEdgePoint()
    {
        // �������� ������� ������ � ������� �����������
        float _cameraHeight = _spawnCamera.orthographicSize * 2f;
        float _cameraWidth = _cameraHeight * _spawnCamera.aspect;

        int _edge = Random.Range(0, 4);

        Vector3 randomPoint = Vector3.zero;

        switch (_edge)
        {
            case 0: // ������� �������
                randomPoint = new Vector3(_playerTransform.position.x + Random.Range(-_cameraWidth / 2f, _cameraWidth / 2f), _playerTransform.position.y, _playerTransform.position.z + (_cameraHeight / 2f) + 3);
                break;
            case 1: // ������ �������
                randomPoint = new Vector3(_playerTransform.position.x + Random.Range(-_cameraWidth / 2f, _cameraWidth / 2f), _playerTransform.position.y, _playerTransform.position.z + (-_cameraHeight / 2f) - 3);
                break;
            case 2: // ����� �������
                randomPoint = new Vector3(_playerTransform.position.x + (-_cameraWidth / 2f) - 3, _playerTransform.position.y, _playerTransform.position.z + Random.Range(-_cameraHeight / 2f, _cameraHeight / 2f));
                break;
            case 3: // ������ �������
                randomPoint = new Vector3(_playerTransform.position.x + (_cameraWidth / 2f) + 3, _playerTransform.position.y, _playerTransform.position.z + Random.Range(-_cameraHeight / 2f, _cameraHeight / 2f));
                break;
        }

        return randomPoint;
    }

}
