using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

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
        int countOfLevelUps = 1;
        if (EXPamount >= 100)
        {
            countOfLevelUps = Mathf.FloorToInt(EXPamount / 100);
            // Обрабатываем несколько уровней
            HandleMultipleLevelUps(countOfLevelUps).Forget();
        }
        else if (_currentEXP >= MaxEXP)
        {
            // Обрабатываем одного уровня
            HandleMultipleLevelUps(countOfLevelUps).Forget();
        }



        UpdateUI();
    }

    private async UniTaskVoid HandleMultipleLevelUps(int countOfLevelUps)
    {
        for (int i = 0; i < countOfLevelUps; i++)
        {
            _currentLVL += 1;
            LVLChanged?.Invoke(_currentLVL);

            if (_currentEXP >= MaxEXP)
            {
                _currentEXP -= MaxEXP;
            }

            // Показываем окно выбора и ЖДЕМ, пока игрок не сделает выбор
            await ShowLevelUpAndWait();

            // Обновляем UI после каждого уровня
            UpdateUI();

            // Небольшая пауза между уровнями (опционально)
            await UniTask.Delay(100);
        }
    }

    private async UniTask ShowLevelUpAndWait()
    {
        LevelUpController levelUpController = FindObjectOfType<LevelUpController>();
        if (levelUpController != null)
        {
            // Ждем, пока игрок не сделает выбор в окне level up
            await levelUpController.ShowLevelUpOptionsAsync();
        }
        else
        {
            Debug.Log("LevelUpController не обнаружен");
        }
    }

    private void UpdateUI()
    {
        if (_expProgressBarImage != null)
        {
            _expProgressBarImage.fillAmount = Mathf.Clamp01(_currentEXP / MaxEXP);
        }

        EXPChanged?.Invoke(_currentEXP);
    }
}