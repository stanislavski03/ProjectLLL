using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRewardInfo : MonoBehaviour
{

    public List<GameObject> ItemsList;
    private ItemDataSO _universalItem = null;
    private ItemDataSO _specialisedItem = null;
    public ItemType _questGiverType;
    [SerializeField] private GameObject _itemRewardCanvas;



    public int numberOfCurrentWeaponList = 2;

    public ItemControllerSO _itemControllerSO;



    private void OnEnable()
    {
        _questGiverType = ItemControllerSO.Instance.questType;
        SetItemsInfo();
        GameStateManager.Instance.PauseForLevelUp();
    }



    public void TransferRandomObjects()
    {
        _universalItem = null;
        _specialisedItem = null;



        if (_itemControllerSO.itemUniversalPool.Count != 0)
        {
            int _randIndex = Random.Range(0, _itemControllerSO.itemUniversalPool.Count);
            _universalItem = _itemControllerSO.itemUniversalPool[_randIndex];

            if (_questGiverType == ItemType.Universal && _itemControllerSO.itemUniversalPool.Count > 1)
            {
                List<ItemDataSO> _list = new List<ItemDataSO>();
                foreach(ItemDataSO g in _itemControllerSO.itemUniversalPool)
                {
                    if(g != _universalItem)
                    {
                        _list.Add(g);
                    }
                }
                int _randSpecialIndex = Random.Range(0, _list.Count);
                _specialisedItem = _list[_randSpecialIndex];
            }
        }
        if (_questGiverType == ItemType.Tecno)
        {
            if (_itemControllerSO.itemTecnoPool.Count != 0)
            {
                int _randIndex = Random.Range(0, _itemControllerSO.itemTecnoPool.Count);
                _specialisedItem = _itemControllerSO.itemTecnoPool[_randIndex];
            }
        }
        else if (_questGiverType == ItemType.Magic)
        {
            if (_itemControllerSO.itemMagicPool.Count != 0)
            {
                int _randIndex = Random.Range(0, _itemControllerSO.itemMagicPool.Count);
                _specialisedItem = _itemControllerSO.itemMagicPool[_randIndex];
            }
        }

    }

    public void SetItemsInfo()
    {
        TransferRandomObjects();
        {

            TextMeshProUGUI[] TMPUniversalItemTitle = ItemsList[0].GetComponentsInChildren<TextMeshProUGUI>(true);
            Button ItemButton = ItemsList[0].GetComponentInChildren<Button>(true);
            Transform imageTransform = ItemsList[0].transform.Find("Image");
            Image ItemImage = imageTransform?.GetComponent<Image>();

            if (_universalItem != null)
            {
                

                TMPUniversalItemTitle[0].text = _universalItem.itemTitle;
                TMPUniversalItemTitle[1].text = _universalItem.description;

                if (ItemImage != null)
                {
                    ItemImage.preserveAspect = true;
                    ItemImage.type = Image.Type.Simple;

                    RectTransform rectTransform = ItemImage.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        rectTransform.sizeDelta = new Vector2(100, 100);
                    }

                    ItemImage.sprite = _universalItem.icon;
                }

                ItemsList[1].SetActive(true);

                ItemButton.onClick.RemoveAllListeners();
                ItemButton.onClick.AddListener(() => OnItemSelected(_universalItem));

            }
            else
            {
                ItemsList[0].SetActive(false);
                ItemsList[1].SetActive(true);

            }

        }
        {
            TextMeshProUGUI[] TMPSpecialisedItemTitle = ItemsList[1].GetComponentsInChildren<TextMeshProUGUI>(true);
            Button ItemButton = ItemsList[1].GetComponentInChildren<Button>(true);
            Transform imageTransform = ItemsList[1].transform.Find("Image");
            Image ItemImage = imageTransform?.GetComponent<Image>();

            if (_specialisedItem != null)
            {
                

                TMPSpecialisedItemTitle[0].text = _specialisedItem.itemTitle;
                TMPSpecialisedItemTitle[1].text = _specialisedItem.description;

                if (ItemImage != null)
                {
                    ItemImage.preserveAspect = true;
                    ItemImage.type = Image.Type.Simple;

                    RectTransform rectTransform = ItemImage.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        rectTransform.sizeDelta = new Vector2(100, 100);
                    }

                    ItemImage.sprite = _specialisedItem.icon;
                }

                ItemButton.onClick.RemoveAllListeners();
                ItemButton.onClick.AddListener(() => OnItemSelected(_specialisedItem));
            }
            else
            {
                ItemsList[1].SetActive(false);
            }

        }

    }

    private void OnItemSelected(ItemDataSO item)
    {
        if (item != null)
        {
            item.OnPick();
            GameStateManager.Instance.ResumeGame();
            _itemRewardCanvas.SetActive(false);
        }


    }

    public void Skip()
    {
        GameStateManager.Instance.ResumeGame();
        _itemRewardCanvas.SetActive(false);
    }

    [ContextMenu("Force Refresh UI")]
    public void ForceRefreshUI()
    {
        SetItemsInfo();
    }
}