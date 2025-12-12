using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance;
    [SerializeField] private float _spawnPatrolPeriod;
    [SerializeField] private float _spawnWavePeriod;
    [SerializeField] private float _difficulty = 0;
    [SerializeField] private float _periodOfGettingPoints;
    [SerializeField] private int _points = 0;
    [SerializeField] private float _pointsToGain = 1;

    private float _timeForPatrolPeriod = 0;
    private float _timeForWavePeriod = 0;
    private float _timeForGettingPoints = 0;

    private bool _spawnIsActive = true;


    private void Awake()
    {
        Instance = this;
    }


    public bool TryToSpendPoints(int pointsToSpend)
    {
        if (pointsToSpend > _points)
            return false;
        else
        {
            _points-=pointsToSpend;
            return true;
        }
    }

    public void GainPoints(int pointsToGain) => _points+=pointsToGain;

    public void GainDifficulty(float gain) => _difficulty += gain;

    public void ChangeActiveEnemySpawn(bool set) => _spawnIsActive = set;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GainPoints(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GainDifficulty(0.5f);
        }


        if (!_spawnIsActive)
            return;


        _timeForWavePeriod += Time.deltaTime;
        _timeForPatrolPeriod += Time.deltaTime;
        _timeForGettingPoints += Time.deltaTime;

        if(_timeForGettingPoints >= _periodOfGettingPoints)
        {
            _timeForGettingPoints = 0;
            GainPoints(Convert.ToInt32(_pointsToGain));
        }

        if (_timeForWavePeriod >= _spawnWavePeriod)
        {
            _timeForWavePeriod = 0;
            int randPointsToSpend = Random.Range(_points / 2, _points + 1);
            if (TryToSpendPoints(randPointsToSpend))
            {
                SpawnWave(randPointsToSpend);
            }
            _pointsToGain += 1 * _difficulty;
        }
        else if (_timeForPatrolPeriod >= _spawnPatrolPeriod)
        {       
            _timeForPatrolPeriod = 0;
            if (TryToSpendPoints(_points))
            {
                SpawnPatrol(_points);
            }
        }


    }

    private void SpawnWave(int pointsToSpend)
    {
        if (pointsToSpend < 10)
        {
            SpawnSmallAmountOfEnemy(pointsToSpend);
        }
        else if (pointsToSpend < 20)
        {
            SpawnMediumAmountOfEnemy(pointsToSpend);
        }
        else
        {
            SpawnBigAmountOfEnemy(pointsToSpend);
        }
    }


    private void SpawnPatrol(int pointsToSpend)
    {

        if (pointsToSpend < 5)
        {
            SpawnSmallAmountOfEnemy(pointsToSpend);
        }
        else if (pointsToSpend < 10) 
        {
            SpawnMediumAmountOfEnemy(pointsToSpend);
        }
        else
        {
            SpawnBigAmountOfEnemy(pointsToSpend);
        }


    }




    private async void SpawnSmallAmountOfEnemy(int pointsToSpend)
    {
        
        while (pointsToSpend >= 0)
        {
            switch (Random.Range(0, 2))
            {
                case 0: EnemySpawner.Instance.SpawnSmallMeleeEnemy(); break;
                case 1: EnemySpawner.Instance.SpawnFastMeleeEnemy(); break;

            }
            pointsToSpend--;
            await UniTask.WaitForSeconds(0.1f);
        }
    }
    private async void SpawnMediumAmountOfEnemy(int pointsToSpend)
    {
        
        while (pointsToSpend >= 0)
        {
            switch (Random.Range(0, 3))
            {
                case 0: EnemySpawner.Instance.SpawnSmallMeleeEnemy(); break;
                case 1: EnemySpawner.Instance.SpawnSmallRangedEnemy(); break;
                case 2: EnemySpawner.Instance.SpawnFastMeleeEnemy(); break;

            }
            pointsToSpend--;
            await UniTask.WaitForSeconds(0.1f);
        }
    }
    private async void SpawnBigAmountOfEnemy(int pointsToSpend)
    {
        //pointsToSpend += Convert.ToInt32((float)pointsToSpend * _difficulty);
        while (pointsToSpend >= 0)
        {
            switch (Random.Range(0, 4))
            {
                case 0: EnemySpawner.Instance.SpawnSmallMeleeEnemy(); break;
                case 1: EnemySpawner.Instance.SpawnSmallRangedEnemy(); break;
                case 2: EnemySpawner.Instance.SpawnFastMeleeEnemy();  break;
                case 3: EnemySpawner.Instance.SpawnStrongMeleeEnemy(); break;
            }
            pointsToSpend--;
            await UniTask.WaitForSeconds(0.1f);
        }
    }


}
