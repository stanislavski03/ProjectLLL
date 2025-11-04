using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEXP : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO _statsSO;
    [SerializeField] private Image _expProgressBarImage;

    private float _currentEXP;
    private float _currentLVL = 1;

    public float MaxEXP => _statsSO.maxEXP;
    public float CurrentEXP => _currentEXP;
    public float CurrentLVL => _currentLVL;

    public event Action<float> EXPChanged;
    public event Action<float> LVLChanged;

    private void Start()
    {
        UpdateUI();
    }

    public void GetEXP(float EXPamount)
    {
        _currentEXP += EXPamount;
        
        if (_currentEXP >= MaxEXP)
        {
            _currentEXP = _currentEXP - MaxEXP;
            LevelUP();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_expProgressBarImage != null)
        {
            _expProgressBarImage.fillAmount = Mathf.Clamp01(_currentEXP / MaxEXP);
        }
        
        EXPChanged?.Invoke(_currentEXP);
    }

    private void LevelUP()
    {
        _currentLVL += 1;
        LVLChanged?.Invoke(_currentLVL);

        // Показываем меню прокачки
        LevelUpController levelUpController = FindObjectOfType<LevelUpController>();
        if (levelUpController != null)
        {
            levelUpController.ShowLevelUpOptions();
        }
    }
}