using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MutationRewardInfo : MonoBehaviour
{

    public List<GameObject> MutationsList;
    private MutationDataSO mutation;
    [SerializeField] private GameObject _mutationRewardCanvas;

    public int numberOfCurrentMutationList = 1;

    private void OnEnable()
    {
        SetMutationInfo();
        GameStateManager.Instance.PauseForLevelUp();
    }



    public void TransferRandomObjects()
    {
       if(MutationControllerSO.Instance.AllMutationsPool != null)
        {
            int _randIndex = Random.Range(0, MutationControllerSO.Instance.AllMutationsPool.Count);
            mutation = MutationControllerSO.Instance.AllMutationsPool[_randIndex];
        }
    }

    public void SetMutationInfo()
    {
        TransferRandomObjects();



        if (mutation != null)
        {
            TextMeshProUGUI[] TMPUniversalMutationTitle = MutationsList[0].GetComponentsInChildren<TextMeshProUGUI>(true);
            Button MutationButton = MutationsList[0].GetComponentInChildren<Button>(true);
            Transform imageTransform = MutationsList[0].transform.Find("Image");
            Image MutationImage = imageTransform?.GetComponent<Image>();

            TMPUniversalMutationTitle[0].text = mutation.mutationTitle;
            TMPUniversalMutationTitle[1].text = mutation.description;

            if (MutationImage != null)
            {
                MutationImage.preserveAspect = true;
                MutationImage.type = Image.Type.Simple;

                RectTransform rectTransform = MutationImage.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(100, 100);
                }

                MutationImage.sprite = mutation.icon;
            }

            MutationButton.onClick.RemoveAllListeners();
            MutationButton.onClick.AddListener(() => OnMutationSelected(mutation));

        }

    }

    private void OnMutationSelected(MutationDataSO mutation)
    {
        if (mutation != null)
        {
            MutationControllerSO.Instance.AddMutation(mutation);
            GameStateManager.Instance.ResumeGame();
            _mutationRewardCanvas.SetActive(false);
        }


    }

    [ContextMenu("Force Refresh UI")]
    public void ForceRefreshUI()
    {
        SetMutationInfo();
    }
}