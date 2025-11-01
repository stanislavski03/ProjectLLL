using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private float _maxHP = 100;


    private float _currentHP;

    public float MaxHP => _maxHP;
    public bool isAlive = true;

    public event Action<float> Changed;

    private void OnEnable()
    {
        _currentHP = _maxHP;
    }



    public void Damage(float damageAmmount)
    {
        _currentHP = Mathf.Clamp(_currentHP - damageAmmount, 0, _maxHP);
        Changed?.Invoke(_currentHP);
        if (_currentHP == 0) Death();
    }

    public void Heal(float healAmmount)
    {
        _currentHP = Mathf.Clamp(_currentHP + healAmmount, 0, _maxHP);
        Changed?.Invoke(_currentHP);
    }


    private void Death()
    {
        Debug.Log("HOLY, HE'S DEAD!");
        Destroy(gameObject);
    }
}
