using UnityEngine;
using UnityEngine.UI;

public class LevelUpController : MonoBehaviour
{
    [SerializeField] private GameObject lvlUpCanvasObject;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private GameObject panelItems;

    private LvlUpWeaponItemsInfo itemsInfo;
    private bool isLevelUpActive = false;

    private void Start()
    {
        FindComponents();
        InitializeUI();
        itemsInfo.SetWeaponList();
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

    public void ShowLevelUpOptions()
    {
        if (isLevelUpActive) return;
        
        isLevelUpActive = true;
        
        GameStateManager.Instance.PauseForLevelUp();
        
        itemsInfo.SetItemsInfo();
        lvlUpCanvasObject.SetActive(true);

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        ActivateButtons();
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
        
        ResumeFromLevelUp();
    }
}