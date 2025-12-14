using UnityEngine;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;

public class LevelUpController : MonoBehaviour
{
    [SerializeField] private GameObject lvlUpCanvasObject;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private GameObject panelItems;

    public AudioClip uiClickClip;
    private LvlUpWeaponItemsInfo itemsInfo;
    private bool isLevelUpActive = false;
    // Инициализация Моста unitask
    private UniTaskCompletionSource<bool> _levelUpCompletionSource;

    public static LevelUpController Instance;

     private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Start()
    {
        FindComponents();
        InitializeUI();
        itemsInfo.SetWeaponList();
        await UniTask.WaitForSeconds(1);
        PlayerEXP.Instance.HandleMultipleLevelUpsWithNoCheck(1).Forget();
    }

    private void FindComponents()
    {
        if (lvlUpCanvasObject == null)
        {
            lvlUpCanvasObject = transform.Find("Canvas")?.gameObject;
            if (lvlUpCanvasObject == null)
            {
                Canvas canvas = GetComponentInChildren<Canvas>();
                if (canvas != null) lvlUpCanvasObject = canvas.gameObject;
            }
        }

        canvasGroup = lvlUpCanvasObject.GetComponent<CanvasGroup>();
        itemsInfo = panelItems.GetComponentInChildren<LvlUpWeaponItemsInfo>();
    }

    private void InitializeUI()
    {
        lvlUpCanvasObject.SetActive(false);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        PrepareButtons();
    }

    public void UiClickSound()
    {
        AudioManager.Instance.PlayClick(uiClickClip);
    }

    private void PrepareButtons()
    {
        foreach (var button in buttons)
        {
            if (button != null)
            {
                var btnComponent = button.GetComponent<Button>();
                if (btnComponent != null) btnComponent.interactable = false;
            }
        }
    }

    public async UniTask ShowLevelUpOptionsAsync()
    {
        if (isLevelUpActive) return;
        isLevelUpActive = true;
        // Создание Моста unitask
        _levelUpCompletionSource = new UniTaskCompletionSource<bool>();
        
        GameStateManager.Instance.PauseForLevelUp();
        
        
        itemsInfo.SetItemsInfo();
        lvlUpCanvasObject.SetActive(true);

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        ActivateButtons();

        // Ждем, пока игрок не сделает выбор
        await _levelUpCompletionSource.Task;
    }

    private void ActivateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];
            if (button != null)
            {
                var btnComponent = button.GetComponent<Button>();
                if (btnComponent != null) 
                {
                    btnComponent.interactable = true;
                }
            }
        }
    }

    public void HideLevelUpOptions(System.Action onComplete = null)
    {
        if (!isLevelUpActive) 
        {
            onComplete?.Invoke();
            return;
        }
        
        isLevelUpActive = false;

        foreach (var button in buttons)
        {
            if (button != null)
            {
                var btnComponent = button.GetComponent<Button>();
                if (btnComponent != null) btnComponent.interactable = false;
            }
        }

        if (lvlUpCanvasObject != null)
        {
            lvlUpCanvasObject.SetActive(false);
        }

        // Завершаем задачу, когда скрываем UI
        _levelUpCompletionSource?.TrySetResult(true);
        
        onComplete?.Invoke();
    }

    public void OnResumeButtonClicked()
    {
        ResumeFromLevelUp();
    }

    private void ResumeFromLevelUp()
    {
        HideLevelUpOptions(() => {
            GameStateManager.Instance.ResumeGame();
        });
    }

    public void OnItemSelected(int itemIndex)
    {
        itemsInfo.SetWeaponList();
        
        // Завершаем задачу при выборе предмета
        _levelUpCompletionSource?.TrySetResult(true);
        ResumeFromLevelUp();
    }
}