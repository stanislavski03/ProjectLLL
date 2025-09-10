using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEXP : MonoBehaviour
{
    [SerializeField] private float _maxEXP;
    [SerializeField] private Image _expProgressBarImage;

    public event Action<float> EXPChanged;
    public event Action<float> LVLChanged;

    private float _currentEXP;
    private float _currentLVL;

    public float MaxEXP => _maxEXP;

    private void OnEnable()
    {
        _currentEXP = 0;
        _currentLVL = 1;
    }

    public void GetEXP(float EXPamount)
    {
        _currentEXP += EXPamount;
        if (_currentEXP >= _maxEXP)
        {
            _currentEXP = _currentEXP - _maxEXP;
            LevelUP();
            LVLChanged?.Invoke(_currentLVL);
        }

        _expProgressBarImage.fillAmount = Mathf.Clamp01(_currentEXP/MaxEXP);

        EXPChanged?.Invoke(_currentEXP);
    }

    private void LevelUP()
    {
        _currentLVL += 1;
        
        // Ставим принудительную паузу при поднятии уровня
        GameStateManager.Instance.SetLevelUpPause();
        
        // Показываем UI для выбора предметов с анимацией
        LevelUpController.Instance.OnLevelUp();
    }
    
    // Метод для вызова из UI когда игрок выбрал предмет
    public void OnLevelUpItemSelected()
    {
        // Снимаем принудительную паузу
        LevelUpController.Instance.ResumePause();
    }
}