using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelUpController : MonoBehaviour
{
    [SerializeField] private Canvas _lvlUpCanvas;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform[] _buttons;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _buttonDelay = 0.1f;
    [SerializeField] private float _buttonScaleDuration = 0.3f;

    private static LevelUpController _instance;
    public static LevelUpController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LevelUpController>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("LevelUpController");
                    _instance = obj.AddComponent<LevelUpController>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Инициализируем DOTween (если еще не инициализирован)
        DOTween.Init();

        // Скрываем canvas при старте
        if (_lvlUpCanvas != null)
        {
            _lvlUpCanvas.enabled = false;
        }
        
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        // Подготавливаем кнопки
        PrepareButtons();
    }

    private void PrepareButtons()
    {
        if (_buttons == null) return;

        foreach (RectTransform button in _buttons)
        {
            if (button != null)
            {
                button.localScale = Vector3.zero;
                
                // Делаем кнопки неинтерактивными изначально
                Button btnComponent = button.GetComponent<Button>();
                if (btnComponent != null)
                {
                    btnComponent.interactable = false;
                }
            }
        }
    }

    public void ResumePause()
    {
        // Плавно скрываем UI перед снятием паузы
        HideLevelUpOptions(() => 
        {
            GameStateManager.Instance.ResumeFromLevelUpPause();
        });
    }

    public void ShowLevelUpOptions()
    {
        if (_lvlUpCanvas != null)
        {
            _lvlUpCanvas.enabled = true;
        }

        // Останавливаем все твины
        DOTween.Kill(_canvasGroup);
        foreach (RectTransform button in _buttons)
        {
            if (button != null) DOTween.Kill(button);
        }

        // Плавное появление canvas группы
        if (_canvasGroup != null)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            _canvasGroup.DOFade(1f, _fadeDuration)
                .SetEase(Ease.OutQuad)
                .OnStart(() =>
                {
                    _canvasGroup.alpha = 0f;
                })
                .OnComplete(() =>
                {
                    // После появления canvas запускаем анимацию кнопок
                    AnimateButtons();
                });
        }
        else
        {
            // Если нет CanvasGroup, просто показываем кнопки
            AnimateButtons();
        }
    }

    private void AnimateButtons()
    {
        if (_buttons == null || _buttons.Length == 0)
            return;

        // Анимация для каждой кнопки с задержкой
        for (int i = 0; i < _buttons.Length; i++)
        {
            RectTransform button = _buttons[i];
            if (button != null)
            {
                // Делаем кнопку интерактивной после анимации
                Button btnComponent = button.GetComponent<Button>();
                
                button.DOScale(Vector3.one, _buttonScaleDuration)
                    .SetDelay(i * _buttonDelay)
                    .SetEase(Ease.OutBack)
                    .OnStart(() =>
                    {
                        button.localScale = Vector3.zero;
                    })
                    .OnComplete(() =>
                    {
                        if (btnComponent != null)
                        {
                            btnComponent.interactable = true;
                        }
                    });
            }
        }
    }

    public void HideLevelUpOptions(System.Action onComplete = null)
    {
        // Делаем кнопки неинтерактивными сразу
        if (_buttons != null)
        {
            foreach (RectTransform button in _buttons)
            {
                if (button != null)
                {
                    Button btnComponent = button.GetComponent<Button>();
                    if (btnComponent != null)
                    {
                        btnComponent.interactable = false;
                    }
                    
                    // Плавное скрытие кнопок
                    button.DOScale(Vector3.zero, _buttonScaleDuration * 0.7f)
                        .SetEase(Ease.InBack);
                }
            }
        }

        // Плавное скрытие canvas
        if (_canvasGroup != null)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            _canvasGroup.DOFade(0f, _fadeDuration * 0.8f)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    if (_lvlUpCanvas != null)
                    {
                        _lvlUpCanvas.enabled = false;
                    }
                    onComplete?.Invoke();
                });
        }
        else
        {
            if (_lvlUpCanvas != null)
            {
                _lvlUpCanvas.enabled = false;
            }
            onComplete?.Invoke();
        }
    }

    // Метод для вызова из PlayerEXP при level up
    public void OnLevelUp()
    {
        ShowLevelUpOptions();
    }

    private void OnDestroy()
    {
        // Очищаем твины при уничтожении объекта
        if (_canvasGroup != null) DOTween.Kill(_canvasGroup);
        if (_buttons != null)
        {
            foreach (RectTransform button in _buttons)
            {
                if (button != null) DOTween.Kill(button);
            }
        }
    }
}