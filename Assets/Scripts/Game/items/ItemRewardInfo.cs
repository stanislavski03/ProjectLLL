using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRewardInfo : MonoBehaviour
{

    public List<GameObject> ItemsList;
    private ItemDataSO _universalItem;
    private ItemDataSO _specialisedItem;
    public ItemType _questGiverType;
    [SerializeField] private GameObject _itemRewardCanvas;



    public int numberOfCurrentWeaponList = 2;

    public ItemControllerSO _itemControllerSO;

    private void OnEnable()
    {
        SetItemsInfo();
        GameStateManager.Instance.PauseForLevelUp();
    }



    public void TransferRandomObjects()
    {
       if(_itemControllerSO.itemUniversalPool != null)
        {
            int _randIndex = Random.Range(0, _itemControllerSO.itemUniversalPool.Count);
            _universalItem = _itemControllerSO.AllItemsPool[_randIndex];
        }
        if (_questGiverType == ItemType.Tecno)
        {
            if (_itemControllerSO.itemTecnoPool != null)
            {
                int _randIndex = Random.Range(0, _itemControllerSO.itemTecnoPool.Count);
                _specialisedItem = _itemControllerSO.itemTecnoPool[_randIndex];
            }
        }
        else if (_questGiverType == ItemType.Magic)
        {
            if (_itemControllerSO.itemMagicPool != null)
            {
                int _randIndex = Random.Range(0, _itemControllerSO.itemMagicPool.Count);
                _specialisedItem = _itemControllerSO.itemMagicPool[_randIndex];
            }
        }
        else
        {
            int _randIndex = Random.Range(0, _itemControllerSO.itemUniversalPool.Count);
            _specialisedItem = _itemControllerSO.AllItemsPool[_randIndex];
        }

    }

    public void SetItemsInfo()
    {
        TransferRandomObjects();



        if (_universalItem != null)
        {
            TextMeshProUGUI[] TMPUniversalItemTitle = ItemsList[0].GetComponentsInChildren<TextMeshProUGUI>(true);
            Button ItemButton = ItemsList[0].GetComponentInChildren<Button>(true);

            TMPUniversalItemTitle[0].text = _universalItem.itemTitle;
            TMPUniversalItemTitle[1].text = _universalItem.description;

            ItemButton.onClick.RemoveAllListeners();
            ItemButton.onClick.AddListener(() => OnItemSelected(_universalItem));

        }

        if(_specialisedItem != null)
        {
            TextMeshProUGUI[] TMPSpecialisedItemTitle = ItemsList[1].GetComponentsInChildren<TextMeshProUGUI>(true);
            Button ItemButton = ItemsList[1].GetComponentInChildren<Button>(true);

            TMPSpecialisedItemTitle[0].text = _specialisedItem.itemTitle;
            TMPSpecialisedItemTitle[1].text = _specialisedItem.description;

            ItemButton.onClick.RemoveAllListeners();
            ItemButton.onClick.AddListener(() => OnItemSelected(_specialisedItem));
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

    [ContextMenu("Force Refresh UI")]
    public void ForceRefreshUI()
    {
        SetItemsInfo();
    }
}