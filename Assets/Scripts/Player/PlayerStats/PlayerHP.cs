using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO _statsSO;


    private float _currentHP;

    public float MaxHP => _statsSO.MaxHP;
    public bool isAlive = true;

    public event Action<float> Changed;

    private void Awake()
    {
        _currentHP = MaxHP;
    }



    public void Damage(float damageAmmount)
    {
        _currentHP = Mathf.Clamp(_currentHP - damageAmmount, 0, MaxHP);
        Changed?.Invoke(_currentHP);
        if (_currentHP == 0) Death();
    }

    public void Heal(float healAmmount)
    {
        _currentHP = Mathf.Clamp(_currentHP + healAmmount, 0, MaxHP);
        Changed?.Invoke(_currentHP);
    }


    private void Death()
    {
        Destroy(gameObject);
    }
}
