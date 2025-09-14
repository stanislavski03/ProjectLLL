using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IGameplaySystem
{
    [SerializeField] private float _speed = 3f;
    public float _playerRange = 1f;

    private Transform _playerTransform;

    private bool isPaused;


    private void Awake()
    {

    }

    private void OnEnable()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void OnDestroy()
    {
    }

    private void FixedUpdate()
    {
        if (isPaused) return;
        
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

    public void SetPaused(bool paused)
    {
        isPaused = paused;
        
        if (paused)
        {
            enabled = false;
        }
        else
        {
            enabled = true;
        }
    }
}
