using DG.Tweening;
using TMPro;
using UnityEngine;

public class UniversalNotification : MonoBehaviour
{
    public static UniversalNotification Instance { get; private set; }
    
    [Header("UI Elements")]
    [SerializeField] private RectTransform notificationPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    [Header("Settings")]
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float hideDelay = 2.5f;
    [SerializeField] private bool autoHide = true;
    
    [Header("Default Position")]
    [SerializeField] private Vector2 hiddenPosition = new Vector2(-1150, -150);
    [SerializeField] private Vector2 shownPosition = new Vector2(-784, -150);
    
    private Sequence currentAnimation;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        // Скрываем панель при старте
        notificationPanel.anchoredPosition = hiddenPosition;
        notificationPanel.gameObject.SetActive(false);
    }
    
    // Основной метод для показа уведомления
    public void ShowNotification(NotificationData data)
    {
        ShowNotificationInternal(
            data.title,
            data.description,
            data.showDuration > 0 ? data.showDuration : hideDelay,
            data.autoHide
        );
    }
    
    // Перегруженные методы для удобства
    public void ShowNotification(string title, string description)
    {
        ShowNotificationInternal(title, description, hideDelay, autoHide);
    }
    
    public void ShowNotification(string title, string description, float customHideDelay)
    {
        ShowNotificationInternal(title, description, customHideDelay, autoHide);
    }
    
    public void ShowNotification(string title, string description, float customHideDelay, bool customAutoHide)
    {
        ShowNotificationInternal(title, description, customHideDelay, customAutoHide);
    }
    
    private void ShowNotificationInternal(string title, string description, float delay, bool shouldAutoHide)
    {
        // Останавливаем текущую анимацию, если есть
        if (currentAnimation != null && currentAnimation.IsActive())
        {
            currentAnimation.Kill();
        }
        
        // Устанавливаем данные
        titleText.text = title;
        descriptionText.text = description;
        
        // Активируем панель
        notificationPanel.gameObject.SetActive(true);
        
        // Создаем анимацию
        currentAnimation = DOTween.Sequence();
        
        // Показываем панель
        currentAnimation.Append(notificationPanel.DOAnchorPos(shownPosition, slideDuration)
            .SetEase(Ease.OutBack));
        
        // Если нужно автоматическое скрытие
        if (shouldAutoHide && delay > 0)
        {
            // Ждем указанное время
            currentAnimation.AppendInterval(delay);
            
            // Скрываем панель
            currentAnimation.Append(notificationPanel.DOAnchorPos(hiddenPosition, slideDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => notificationPanel.gameObject.SetActive(false)));
        }
    }
    
    // Метод для принудительного скрытия уведомления
    public void HideNotification()
    {
        if (currentAnimation != null && currentAnimation.IsActive())
        {
            currentAnimation.Kill();
        }
        
        notificationPanel.DOAnchorPos(hiddenPosition, slideDuration / 2f)
            .SetEase(Ease.InBack)
            .OnComplete(() => notificationPanel.gameObject.SetActive(false));
    }
    
    // Метод для обновления контента без повторной анимации
    public void UpdateNotificationContent(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
    }
    
    // Метод для изменения позиции отображения
    public void SetNotificationPosition(Vector2 hiddenPos, Vector2 shownPos)
    {
        hiddenPosition = hiddenPos;
        shownPosition = shownPos;
    }
    
    // Метод для проверки, активно ли уведомление в данный момент
    public bool IsNotificationActive()
    {
        return notificationPanel.gameObject.activeSelf;
    }
    
    // Метод для немедленного показа (без анимации)
    public void ShowImmediate(string title, string description)
    {
        if (currentAnimation != null && currentAnimation.IsActive())
        {
            currentAnimation.Kill();
        }
        
        titleText.text = title;
        descriptionText.text = description;
        notificationPanel.anchoredPosition = shownPosition;
        notificationPanel.gameObject.SetActive(true);
    }
    
    // Метод для немедленного скрытия (без анимации)
    public void HideImmediate()
    {
        if (currentAnimation != null && currentAnimation.IsActive())
        {
            currentAnimation.Kill();
        }
        
        notificationPanel.anchoredPosition = hiddenPosition;
        notificationPanel.gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        if (currentAnimation != null)
        {
            currentAnimation.Kill();
        }
    }
}

// Класс для передачи данных уведомления
[System.Serializable]
public class NotificationData
{
    public string title;
    public string description;
    public float showDuration = 2.5f; // 0 = использовать настройки по умолчанию
    public bool autoHide = true;
    
    public NotificationData(string title, string description)
    {
        this.title = title;
        this.description = description;
    }
    
    public NotificationData(string title, string description, float showDuration, bool autoHide)
    {
        this.title = title;
        this.description = description;
        this.showDuration = showDuration;
        this.autoHide = autoHide;
    }
}