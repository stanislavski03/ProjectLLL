using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    public float _playerRange = 1f;

    private Transform _playerTransform;


    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        CountdownController.OnCountdownStarted += OnCountdownStarted;
        CountdownController.OnCountdownFinished += OnCountdownFinished;
    }

    private void OnEnable()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        CountdownController.OnCountdownStarted -= OnCountdownStarted;
        CountdownController.OnCountdownFinished -= OnCountdownFinished;
    }

    private void FixedUpdate()
    {
        LookAt();
        GoFoward();
    }

    private void LookAt()
    {
        Vector3 _lookTarget = new Vector3(_playerTransform.position.x, transform.position.y, _playerTransform.position.z);
        transform.LookAt(_lookTarget);
    }
    private void GoFoward()
    {
        if (Vector3.Distance(transform.position, _playerTransform.position) > _playerRange)
            transform.position += transform.forward * _speed * Time.fixedDeltaTime;
    }

    private void OnCountdownStarted()
    {
        enabled = false;
    }

    private void OnCountdownFinished()
    {
        if (GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
        {
            enabled = true;
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        if (newGameState == GameState.Paused || newGameState == GameState.LevelUpPaused)
        {
            enabled = false;
        }
        else if (newGameState == GameState.Gameplay)
        {
            enabled = false;
        }
    }
}
