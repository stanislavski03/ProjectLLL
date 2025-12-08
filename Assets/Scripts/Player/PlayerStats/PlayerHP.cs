using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO _statsSO;
    [SerializeField] private GameObject _deathPanel;


    public float _currentHP;

    public float MaxHP;
    public bool isAlive = true;

    private float healCount;

    public AudioClip hitClip;

    public event Action<float> Changed;
    public event Action<float> MaxHpChanged;

    private void Awake()
    {
        MaxHP = _statsSO.MaxHP;
        _currentHP = MaxHP;
    }

    private void OnEnable()
    {
        PlayerStatsSO.Instance._maxHpChanged += maxHPChanged;
    }

    private void OnDisable()
    {
        PlayerStatsSO.Instance._maxHpChanged -= maxHPChanged;
    }

    public void Damage(float damageAmmount)
    {
        ItemControllerSO.Instance.ActivateOnDamageGiveEvent();
        if (PlayerStatsSO.Instance.invincibility == false)
        {
            AudioManager.Instance.PlayHit(hitClip);
            PlayerHitEffect.Instance.TakeHit();
            _currentHP = Mathf.Clamp(_currentHP - damageAmmount, 0, MaxHP);
            Changed?.Invoke(_currentHP);
            if (_currentHP == 0) Death();
        }
    }

    public void Heal(float healAmmount)
    {
        _currentHP = Mathf.Clamp(_currentHP + healAmmount, 0, MaxHP);
        Changed?.Invoke(_currentHP);
    }

    public void maxHPChanged(float maxHP)
    {
        healCount = _statsSO.MaxHP - MaxHP;
        MaxHP = _statsSO.MaxHP;
        MaxHpChanged?.Invoke(MaxHP);
        Heal(healCount);
    }


    private void Death()
    {
        
        _deathPanel.SetActive(true);
        GameStateManager.Instance.PauseForLevelUp();
        Destroy(gameObject);
    }
}
