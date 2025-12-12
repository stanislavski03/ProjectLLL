using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransitionQuestStartPanel : MonoBehaviour
{
    public static TransitionQuestStartPanel Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI questTitleText;
    [SerializeField] private TextMeshProUGUI questDescriptionText;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;

    [Header("Settings")]
    [SerializeField] private string warningMessage = "Этот квест перенесет вас на следующий уровень!\nПродолжить?";
    
    private QuestData currentQuest;
    private QuestGiver currentQuestGiver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        panel.SetActive(false);
        
        // Настройка кнопок
        acceptButton.onClick.AddListener(AcceptQuest);
        declineButton.onClick.AddListener(DeclineQuest);
    }

    private void Start()
    {
        // Скрываем панель при старте
        HidePanel();
    }

    public void ShowTransitionQuestConfirmation(QuestData quest, QuestGiver questGiver)
    {
        if (quest == null || questGiver == null)
        {
            Debug.LogError("Quest or QuestGiver is null!");
            return;
        }

        currentQuest = quest;
        currentQuestGiver = questGiver;

        // Заполняем информацию о квесте
        questTitleText.text = quest.QuestName;
        questDescriptionText.text = quest.QuestDescription;
        warningText.text = warningMessage;

        // Пауза игры через LevelUpPause
        GameStateManager.Instance.PauseForLevelUp();
        
        // Показываем панель
        panel.SetActive(true);
        
        // Активируем курсор
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
    }

    private void AcceptQuest()
    {
        if (currentQuest != null && currentQuestGiver != null)
        {
            // Начинаем квест
            currentQuest._questGiver = currentQuestGiver;
            currentQuest.OnQuestStart();
            
            // Регистрируем квест
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.RegisterQuest(currentQuest);
            }

            // Показываем уведомление о принятии квеста
            if (QuestNotification.Instance != null)
            {
                QuestNotification.Instance.ShowQuestAccepted(currentQuest.QuestName);
            }

            // Выключаем возможность взаимодействия с квестодателем
            currentQuestGiver.MakeNonReady();
            currentQuestGiver._canBeInteractedWith = false;
            
            // Активируем аниматор квестодателя
            if (currentQuestGiver._questAnimator != null)
            {
                currentQuestGiver._questAnimator.SetBool("IsActive", true);
            }
        }

        // Скрываем панель и возобновляем игру
        HidePanel();
    }

    private void DeclineQuest()
    {
        // Просто скрываем панель без принятия квеста
        HidePanel();

    }

    public void HidePanel()
    {
        panel.SetActive(false);
        currentQuest = null;
        currentQuestGiver = null;
        
        // Возобновляем игру, если это LevelUpPause
        if (GameStateManager.Instance != null && 
            GameStateManager.Instance.CurrentPauseType == GameStateManager.PauseType.LevelUpPause)
        {
            GameStateManager.Instance.ResumeGame();
        }
        
        // Восстанавливаем курсор (если не в меню паузы)
        if (GameStateManager.Instance != null && !GameStateManager.Instance.IsPaused)
        {
            // Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        // Обработка Escape для закрытия панели
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            DeclineQuest();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}