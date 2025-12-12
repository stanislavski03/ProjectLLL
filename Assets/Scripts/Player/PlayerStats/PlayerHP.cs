using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO _statsSO;

     public static PlayerHP Instance { get; private set; }

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        MaxHP = _statsSO.MaxHP;
        _currentHP = MaxHP;
    }


    public float _currentHP;

    public float MaxHP;
    public bool isAlive = true;

    private float healCount;

    public AudioClip hitClip;

    public event Action<float> Changed;
    public event Action<float> MaxHpChanged;

    private void OnEnable()
    {
        PlayerStatsSO.Instance._maxHpChanged += maxHPChanged;
    }

    private void OnDisable()
    {
        PlayerStatsSO.Instance._maxHpChanged -= maxHPChanged;
    }

    public void DamageInPercent(float damageAmmount)
    {
        ItemControllerSO.Instance.ActivateOnDamageGiveEvent();
        if (PlayerStatsSO.Instance.invincibility == false)
        {
            AudioManager.Instance.PlayHit(hitClip);
            PlayerHitEffect.Instance.TakeHit();
            _currentHP = Mathf.Clamp(_currentHP - damageAmmount/100 * MaxHP, 0, MaxHP);
            Changed?.Invoke(_currentHP);
            if (_currentHP / MaxHP * 100 <= 50)
            {
                AudioManager.Instance.SetMusicPitch(_currentHP / MaxHP + 0.4f);
            }
            else
            {
                AudioManager.Instance.ResetMusicPitch();
            }
            FindObjectOfType<FinalVignette>()?.TriggerDamageVignette();

            if (_currentHP == 0) Death();
        }
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
            if(_currentHP/MaxHP * 100 <= 50)
            {
                AudioManager.Instance.SetMusicPitch(_currentHP/MaxHP + 0.4f);
            }
            else
            {
                AudioManager.Instance.ResetMusicPitch();
            }
            FindObjectOfType<FinalVignette>()?.TriggerDamageVignette();

            if (_currentHP == 0) Death();
        }
    }

    public void Heal(float healAmmount)
    {
        _currentHP = Mathf.Clamp(_currentHP + healAmmount, 0, MaxHP);
        if(_currentHP/MaxHP * 100 <= 50)
            {
                AudioManager.Instance.SetMusicPitch(_currentHP/MaxHP + 0.4f);
            }
            else
            {
                AudioManager.Instance.ResetMusicPitch();
            }
            FindObjectOfType<FinalVignette>()?.TriggerHealVignette();
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
        Player.Instance.TriggerLoose();
        Destroy(gameObject);
    }
}
