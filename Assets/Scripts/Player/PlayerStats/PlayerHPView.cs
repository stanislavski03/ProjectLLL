using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHPView : MonoBehaviour
{
    [SerializeField] private PlayerHP _health;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Image _healthProgressBarImage;


    void Start()
    {
        _healthText.text = _health.MaxHP.ToString("0") + " / " + _health.MaxHP.ToString("0");
    }

    private void OnEnable()
    {
        _health.Changed += TakeDamage;
        _health.MaxHpChanged += ChangeMaxHPText;
    }
    
    private void OnDisable()
    {
        _health.Changed -= TakeDamage;
        _health.MaxHpChanged -= ChangeMaxHPText;
    }

    private void TakeDamage(float currentHP)
    {
        _health._currentHP = currentHP;
        _healthText.text = currentHP.ToString("0") + " / " + _health.MaxHP.ToString("0");

        if (_healthProgressBarImage != null)
        {
            _healthProgressBarImage.fillAmount = Mathf.Clamp01(_health._currentHP / _health.MaxHP);
        }
    }
    
    private void ChangeMaxHPText(float maxHP)
    {
        _healthText.text = _health._currentHP.ToString("0") + " / " + _health.MaxHP.ToString("0");

        if (_healthProgressBarImage != null)
        {
            _healthProgressBarImage.fillAmount = Mathf.Clamp01(_health._currentHP / _health.MaxHP);
        }
    }
 }
