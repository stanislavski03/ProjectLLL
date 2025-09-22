using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEXP : MonoBehaviour
{
    [SerializeField] private float _maxEXP;
    [SerializeField] private Image _expProgressBarImage;

    private float _currentEXP;
    private float _currentLVL;

    public float MaxEXP => _maxEXP;

    public event Action<float> EXPChanged;
    public event Action<float> LVLChanged;

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

        _expProgressBarImage.fillAmount = Mathf.Clamp01(_currentEXP / MaxEXP);

        EXPChanged?.Invoke(_currentEXP);
    }

    private void LevelUP()
    {
        _currentLVL += 1;

        // ВАЖНО: Сначала меняем состояние
        GameStateManager.Instance.RequestLevelUp();

        // Затем показываем UI (после перехода в LevelUpState)
        StartCoroutine(ShowLevelUpUIAfterDelay());
    }

    private IEnumerator ShowLevelUpUIAfterDelay()
    {
        // Ждем один кадр, чтобы состояние успело поменяться
        yield return null;

        LevelUpController levelUpController = FindObjectOfType<LevelUpController>();
        if (levelUpController != null && GameStateManager.Instance.IsCurrentState<LevelUpState>())
        {
            levelUpController.OnLevelUp();
        }
    }

    // В методе выбора предмета:
    public void OnLevelUpItemSelected()
    {
        // ВАЖНО: Вызываем Resume через GameStateManager
        GameStateManager.Instance.RequestResume();
    }
}