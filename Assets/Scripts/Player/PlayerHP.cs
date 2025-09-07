using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private float _maxHP = 100;

    [SerializeField] private float _physicDef = 0;
    [SerializeField] private float _freezeDef = 0;
    [SerializeField] private float _fireDef = 0;
    [SerializeField] private float _energyDef = 0;

    private float _currentHP;

    public event Action<float> Changed;

    public float MaxHP => _maxHP;
    public bool isAlive = true;

    private void OnEnable()
    {
        _currentHP = _maxHP;
    }

    public void Damage(float damageAmmount, int damageType)
    {
        switch (damageType)
        {
            case 0:
                _currentHP -= damageAmmount * (_physicDef / 100);
                break;
            case 1:
                _currentHP -= damageAmmount * (_freezeDef / 100);
                break;
            case 2:
                _currentHP -= damageAmmount * (_fireDef / 100);
                break;
            case 3:
                _currentHP -= damageAmmount * (_energyDef / 100);
                break;
            default:
                _currentHP -= damageAmmount;
                break;
        }
        _currentHP = Mathf.Clamp(_currentHP - damageAmmount, 0, _maxHP);
        Changed?.Invoke(_currentHP);
        if (_currentHP == 0) Death();
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


    //����������� ��� �������� ����� � ����� �����
    //private void OnMouseDown()
    //{
    //    Damage(10, 0);
    //}
}
