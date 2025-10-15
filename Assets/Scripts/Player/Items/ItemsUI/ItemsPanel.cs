using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class ItemsPanel : MonoBehaviour
{
    public static ItemsPanel Instance { get; private set; }

    public List<ItemDataSO> AllPlayerItems;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetItemsInfo()
    {
        AllPlayerItems = ItemControllerSO.Instance.itemAllTypesPool;

        Transform[] items = new Transform[transform.childCount];

        int indexOfCurrentItemForSetACtive = 0;


        for (int i = 0; i < transform.childCount; i++)
        {
            items[i] = transform.GetChild(i);
        }

        for (int i = 0; i < AllPlayerItems.Count; i++)
        {

            if (AllPlayerItems[i] != null)
            {
                items[i].GetComponent<UnityEngine.UI.Image>().sprite = AllPlayerItems[i].icon;
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (i >= AllPlayerItems.Count)
            {
                return;
            }
            if (AllPlayerItems[i] != null)
            {
                items[indexOfCurrentItemForSetACtive].gameObject.SetActive(true);
                indexOfCurrentItemForSetACtive++;
            }
        }
    }

    public void ClearItemList()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }


}
