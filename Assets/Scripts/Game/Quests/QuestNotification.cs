using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestNotification : MonoBehaviour
{
    public static QuestNotification Instance { get; private set; }
    
    [Header("UI Elements")]
    [SerializeField] private RectTransform notificationPanel;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image iconImage;
    
    [Header("Settings")]
    // [SerializeField] private float showDuration = 3f;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float hideDelay = 2.5f;
    
    [Header("Colors")]
    [SerializeField] private Color acceptedColor = new Color(0.2f, 0.8f, 0.2f, 0.9f); // Зеленый
    [SerializeField] private Color completedColor = new Color(0.9f, 0.9f, 0.2f, 0.9f); // Желтый
    [SerializeField] private Color turnedInColor = new Color(0.2f, 0.5f, 1f, 0.9f); // Синий
    [SerializeField] private Color failedColor = new Color(1f, 0.3f, 0.3f, 0.9f); // Красный
    
    [Header("Icons")]
    [SerializeField] private Sprite acceptedIcon;
    [SerializeField] private Sprite completedIcon;
    [SerializeField] private Sprite turnedInIcon;
    [SerializeField] private Sprite failedIcon;
    
    private Vector2 hiddenPosition;
    private Vector2 shownPosition;
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
        
        // Инициализация позиций
        hiddenPosition = new Vector2(0, 150);
        shownPosition = new Vector2(0, -50);
        
        // Скрываем панель при старте
        notificationPanel.anchoredPosition = hiddenPosition;
        notificationPanel.gameObject.SetActive(false);
    }
    
    public void ShowQuestAccepted(string questName)
    {
        ShowNotification(
            "КВЕСТ ПРИНЯТ",
            questName,
            acceptedColor,
            acceptedIcon
        );
    }
    
    public void ShowQuestCompleted(string questName)
    {
        ShowNotification(
            "КВЕСТ ВЫПОЛНЕН",
            questName,
            completedColor,
            completedIcon
        );
    }
    
    public void ShowQuestTurnedIn(string questName)
    {
        ShowNotification(
            "НАГРАДА ПОЛУЧЕНА",
            questName,
            turnedInColor,
            turnedInIcon
        );
    }
    
    public void ShowQuestFailed(string questName)
    {
        ShowNotification(
            "КВЕСТ ПРОВАЛЕН",
            questName,
            failedColor,
            failedIcon
        );
    }
    
    public void ShowQuestProgress(string questName, int progress)
    {
        ShowNotification(
            "ПРОГРЕСС КВЕСТА",
            $"{questName}\nПрогресс: {progress}%",
            completedColor,
            completedIcon
        );
    }
    
    private void ShowNotification(string title, string description, Color color, Sprite icon)
    {
        // Останавливаем текущую анимацию, если есть
        if (currentAnimation != null && currentAnimation.IsActive())
        {
            currentAnimation.Kill();
        }
        
        // Устанавливаем данные
        titleText.text = title;
        descriptionText.text = description;
        backgroundImage.color = color;
        
        if (icon != null && iconImage != null)
        {
            iconImage.sprite = icon;
            iconImage.gameObject.SetActive(true);
        }
        else
        {
            iconImage.gameObject.SetActive(false);
        }
        
        // Активируем панель
        notificationPanel.gameObject.SetActive(true);
        
        // Создаем анимацию
        currentAnimation = DOTween.Sequence();
        
        // Показываем панель
        currentAnimation.Append(notificationPanel.DOAnchorPos(shownPosition, slideDuration)
            .SetEase(Ease.OutBack));
        
        // Ждем
        currentAnimation.AppendInterval(hideDelay);
        
        // Скрываем панель
        currentAnimation.Append(notificationPanel.DOAnchorPos(hiddenPosition, slideDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() => notificationPanel.gameObject.SetActive(false)));
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
    
    private void OnDestroy()
    {
        if (currentAnimation != null)
        {
            currentAnimation.Kill();
        }
    }
}