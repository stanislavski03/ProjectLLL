using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }
    public Camera _spawnCamera;
    private Transform _playerTransform;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
        _spawnCamera = Camera.main;
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        SpawnSmallMeleeEnemy(GetRandomEdgePoint());
    //    }
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        SpawnSmallRangedEnemy(GetRandomEdgePoint());
    //    }
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        SpawnFastMeleeEnemy(GetRandomEdgePoint());
    //    }
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        SpawnStrongMeleeEnemy(GetRandomEdgePoint());
    //    }

    //}

    public Vector3 GetRandomEdgePoint()
    {
        float _cameraHeight = _spawnCamera.orthographicSize * 2f;
        float _cameraWidth = _cameraHeight * _spawnCamera.aspect;
        int _edge = Random.Range(0, 4);
        Vector3 randomPoint = Vector3.zero;

        switch (_edge)
        {
            case 0:
                randomPoint = new Vector3(
                    _playerTransform.position.x + Random.Range(-(10 + _cameraWidth) / 2f, (10 + _cameraWidth) / 2f),
                    _playerTransform.position.y,
                    _playerTransform.position.z + (_cameraHeight / 2f) + 12);
                break;
            case 1:
                randomPoint = new Vector3(
                    _playerTransform.position.x + Random.Range(-_cameraWidth / 2f, _cameraWidth / 2f),
                    _playerTransform.position.y,
                    _playerTransform.position.z + (-_cameraHeight / 2f) - 3);
                break;
            case 2:
                randomPoint = new Vector3(
                    _playerTransform.position.x + (-_cameraWidth / 2f) - 10,
                    _playerTransform.position.y,
                    _playerTransform.position.z + Random.Range(-(10 + _cameraHeight) / 2f, (10 + _cameraHeight) / 2f));
                break;
            case 3:
                randomPoint = new Vector3(
                    _playerTransform.position.x + (_cameraWidth / 2f) + 10,
                    _playerTransform.position.y,
                    _playerTransform.position.z + Random.Range(-(10 + _cameraHeight) / 2f, (10 + _cameraHeight) / 2f));
                break;
        }

        return randomPoint;

    }

    public void SpawnSmallMeleeEnemy(Vector3 position)
    {
        EnemyPoolList.instance._smallMeleeEnemy.GetEnemy(position);
    }
    public void SpawnSmallRangedEnemy(Vector3 position)
    {
        EnemyPoolList.instance._smallRangedEnemy.GetEnemy(position);
    }
    public void SpawnFastMeleeEnemy(Vector3 position)
    {
        EnemyPoolList.instance._fastMeleeEnemy.GetEnemy(position);
    }
    public void SpawnStrongMeleeEnemy(Vector3 position)
    {
        EnemyPoolList.instance._strongMeleeEnemy.GetEnemy(position);
    }



    public void SpawnSmallMeleeEnemy()
    {
        SpawnSmallMeleeEnemy(GetRandomEdgePoint());
    }
    public void SpawnSmallRangedEnemy()
    {
        SpawnSmallRangedEnemy(GetRandomEdgePoint());
    }
    public void SpawnFastMeleeEnemy()
    {
        SpawnFastMeleeEnemy(GetRandomEdgePoint());
    }
    public void SpawnStrongMeleeEnemy()
    {
        SpawnStrongMeleeEnemy(GetRandomEdgePoint());
    }

}
