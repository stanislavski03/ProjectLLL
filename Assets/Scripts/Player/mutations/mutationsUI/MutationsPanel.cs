using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MutationsPanel : MonoBehaviour
{
    public static MutationsPanel Instance { get; private set; }
    
    public MutationControllerSO controller;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            controller = MutationControllerSO.Instance;
        }
    }

    public void SetMutationInfo()
    {
        // Получаем список дочерних объектов с тегом uiScriptElement
        List<Transform> items = new List<Transform>();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.CompareTag("uiScriptElement"))
            {
                items.Add(child);
            }
        }
        
        // Получаем уникальные мутации игрока
        var uniqueMutations = controller.GetUniquePlayerMutations();
        
        // Сначала все скрываем
        foreach (var item in items)
        {
            item.gameObject.SetActive(false);
        }
        
        // Показываем и настраиваем только нужное количество
        for (int i = 0; i < Mathf.Min(uniqueMutations.Count, items.Count); i++)
        {
            var mutationEntry = uniqueMutations[i];
            Transform item = items[i];
            
            if (mutationEntry != null && mutationEntry.mutation != null)
            {
                // Устанавливаем иконку
                Image image = item.GetComponent<Image>();
                if (image != null)
                {
                    image.sprite = mutationEntry.mutation.icon;
                }
                
                // Устанавливаем количество
                TextMeshProUGUI text = item.GetComponentInChildren<TextMeshProUGUI>(false);
                if (text != null)
                {
                    text.text = mutationEntry.count > 0 ? $"{mutationEntry.count}" : "";
                }
                
                // Показываем элемент
                item.gameObject.SetActive(true);
            }
        }
    }

    public void ClearMutationList()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("uiScriptElement"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    // Метод для обновления отображения извне
    public void UpdateMutationsDisplay()
    {
        SetMutationInfo();
    }
}