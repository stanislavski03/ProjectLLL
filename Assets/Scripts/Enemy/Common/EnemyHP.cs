using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHP : MonoBehaviour
{

    [SerializeField] private float _maxHP = 100;

    [SerializeField] private float _freezeDef = 0;
    [SerializeField] private float _fireDef = 0;
    [SerializeField] private float _electroDef = 0;

    [SerializeField] private GameObject _expPrefab;
    [SerializeField] private float _expDropPercent = 10;
    [SerializeField] private float _expAutodropAmount = 10;

    public event Action<float> onDamage;

    private PlayerEXP _playerEXP;

    private float _currentHP;
    public float GetHP()
    {
        return _currentHP;
    }
    public float GetMaxHP()
    {
        return _maxHP;
    }

    private void OnEnable()
    {
        _currentHP = _maxHP;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerEXP = player.GetComponent<PlayerEXP>();
        }
    }

    public void Damage(float damageAmmount, int damageType)
    {
        switch (damageType)
        {
            case 0:
                _currentHP -= damageAmmount * _freezeDef / 100;
                break;
            case 1:
                _currentHP -= damageAmmount * _fireDef / 100;
                break;
            case 2:
                _currentHP -= damageAmmount * _electroDef / 100;
                break;
            default:
                _currentHP -= damageAmmount;
                break;
        }
        _currentHP -= damageAmmount;
        if (_currentHP <= 0) Death();
        Debug.Log(_currentHP);
        onDamage?.Invoke(damageAmmount);
    }


    public void Damage(float damageAmmount)
    {
        _currentHP -= damageAmmount;
        if (_currentHP <= 0) Death();
    }

    public void Heal(float healAmmount)
    {
        _currentHP += healAmmount;
    }

    private void Death()
    {
        Debug.Log("HOLY, HE'S DEAD!");
        if(UnityEngine.Random.Range(1f, 100f) <= _expDropPercent)
            Instantiate(_expPrefab, gameObject.transform.position, Quaternion.identity);
        _playerEXP.GetEXP(_expAutodropAmount);
        Destroy(gameObject);
    }
}
