using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

[System.Serializable]
public class UIElementData
{
    public Transform element;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public float duration = 1f;
    public Ease easeType = Ease.OutQuad;
    public bool useLocalPosition = false;
}

public class FlexibleUIMover : MonoBehaviour
{
    [Header("Элементы UI")]
    public List<UIElementData> uiElements = new List<UIElementData>();
    
    [Header("Настройки анимации")]
    public float defaultDuration = 1f;
    public Ease defaultEaseType = Ease.OutQuad;
    
    [Header("Управление")]
    public KeyCode switchKey = KeyCode.Tab;
    public bool enableTabSwitching = true;
    
    private bool isAtTargetPosition = false;
    private bool isAnimating = false;
    private List<Tween> activeTweens = new List<Tween>();

    void Update()
    {
        if (enableTabSwitching && Input.GetKeyDown(switchKey) && !isAnimating)
        {
            SwitchAllElements();
        }
    }
    
    public void SwitchAllElements()
    {
        if (isAnimating) return;
        
        isAnimating = true;
        activeTweens.Clear();
        
        int animationsCompleted = 0;
        int totalAnimations = uiElements.Count;
        
        foreach (var elementData in uiElements)
        {
            if (elementData.element == null) 
            {
                animationsCompleted++;
                continue;
            }
            
            Vector3 targetPosition = isAtTargetPosition ? elementData.startPosition : elementData.targetPosition;
            float duration = elementData.duration > 0 ? elementData.duration : defaultDuration;
            Ease easeType = elementData.easeType;
            
            Tween tween;
            
            if (elementData.useLocalPosition)
            {
                tween = elementData.element.DOLocalMove(targetPosition, duration)
                          .SetEase(easeType)
                          .SetUpdate(true); // Важно! Работает при TimeScale = 0
            }
            else
            {
                tween = elementData.element.DOMove(targetPosition, duration)
                          .SetEase(easeType)
                          .SetUpdate(true); // Важно! Работает при TimeScale = 0
            }
            
            tween.OnComplete(() => 
            {
                animationsCompleted++;
                if (animationsCompleted >= totalAnimations)
                {
                    isAnimating = false;
                    isAtTargetPosition = !isAtTargetPosition;
                    activeTweens.Clear();
                }
            });
            
            activeTweens.Add(tween);
        }
    }
    
    // Ручное управление
    public void MoveToTargetPositions()
    {
        if (isAtTargetPosition || isAnimating) return;
        SwitchAllElements();
    }
    
    public void MoveToStartPositions()
    {
        if (!isAtTargetPosition || isAnimating) return;
        SwitchAllElements();
    }
    
    public void StopAllAnimations()
    {
        foreach (var tween in activeTweens)
        {
            if (tween != null && tween.IsActive())
            {
                tween.Kill();
            }
        }
        activeTweens.Clear();
        isAnimating = false;
    }
    
    public bool IsAtTargetPosition() => isAtTargetPosition;
    public bool IsAnimating() => isAnimating;
}