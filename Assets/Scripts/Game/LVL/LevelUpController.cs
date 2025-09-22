using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelUpController : MonoBehaviour, IPausable
{
    [SerializeField] private Canvas _lvlUpCanvas;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform[] _buttons;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _buttonDelay = 0.1f;
    [SerializeField] private float _buttonScaleDuration = 0.3f;
    [SerializeField] private GameObject _PanelItems;

    private bool isPaused;

    private void Awake()
    {
        _lvlUpCanvas.gameObject.SetActive(true);
        // Инициализируем DOTween
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

    private void Start()
    {
        // Автоматически регистрируемся в системе паузы
        // GameStateManager найдет нас через интерфейс IPausable
    }

    private void PrepareButtons()
    {
        if (_buttons == null) return;

        foreach (RectTransform button in _buttons)
        {
            if (button != null)
            {
                button.localScale = Vector3.zero;

                Button btnComponent = button.GetComponent<Button>();
                if (btnComponent != null)
                {
                    btnComponent.interactable = false;
                }
            }
        }
    }

    // Новый метод для закрытия паузы через UI кнопку
    public void ResumeGame()
    {
        Debug.Log("ResumeGame called from UI button");

        // Проверяем, что мы именно в LevelUpState
        if (GameStateManager.Instance.IsCurrentState<LevelUpState>())
        {
            HideLevelUpOptions(() =>
            {
                // ВАЖНО: Вызываем RequestResume у текущего состояния
                GameStateManager.Instance.RequestResume();
            });
        }
    }

    public void ShowLevelUpOptions()
    {
        if (_lvlUpCanvas != null)
        {
            _lvlUpCanvas.enabled = true;
        }

        DOTween.Kill(_canvasGroup);
        foreach (RectTransform button in _buttons)
        {
            if (button != null) DOTween.Kill(button);
        }

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
                    AnimateButtons();
                });
        }
        else
        {
            AnimateButtons();
        }
    }

    private void AnimateButtons()
    {
        if (_buttons == null || _buttons.Length == 0)
            return;

        for (int i = 0; i < _buttons.Length; i++)
        {
            RectTransform button = _buttons[i];
            if (button != null)
            {
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

                    button.DOScale(Vector3.zero, _buttonScaleDuration * 0.7f)
                        .SetEase(Ease.InBack);
                }
            }
        }

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

    // Реализация интерфейса IPausable
    public void SetPaused(bool paused)
    {
        isPaused = paused;

        if (paused)
        {
            // При паузе скрываем UI (если был показан)
            HideLevelUpOptions();
        }
        else
        {
            // При снятии паузы также скрываем на всякий случай
            HideLevelUpOptions();
        }
    }

    // Метод для вызова из PlayerEXP при level up
    public void OnLevelUp()
    {
        ShowLevelUpOptions();
        _PanelItems.GetComponent<LvlUpItemsInfo>().SetItemsInfo();
    }

    private void OnDestroy()
    {
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