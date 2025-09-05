using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _playerRange = 1f;

    private Transform _playerTransform;
    

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnEnable()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
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

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
