using UnityEngine;
using System.Collections.Generic;

public class PlayerHitEffect : MonoBehaviour
{
    [Header("Renderer References")]
    public List<Renderer> playerRenderers = new List<Renderer>();
    
    [Header("Effect Settings")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.3f;
    
    private bool isFlashing = false;
    private float flashTimer = 0f;
    private int flashState = 0; // 0=original, 1=red
    
    public static PlayerHitEffect Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        if (isFlashing)
        {
            flashTimer += Time.deltaTime;
            
            if (flashTimer >= flashDuration)
            {
                isFlashing = false;
                SetColorToOriginal();
            }
            else
            {
                // Автоматическое переключение между цветами
                float interval = flashDuration / flashDuration; // 2 вспышки
                int newState = (Mathf.FloorToInt(flashTimer / interval) % 2 == 0) ? 1 : 0;
                
                if (newState != flashState)
                {
                    flashState = newState;
                    if (flashState == 1)
                        SetColorToRed();
                    else
                        SetColorToOriginal();
                }
            }
        }
    }
    
    public void TakeHit()
    {
        isFlashing = true;
        flashTimer = 0f;
        flashState = 1;
        SetColorToRed();
    }
    
    private void SetColorToRed()
    {
        foreach (var renderer in playerRenderers)
        {
            if (renderer == null) continue;
            
            var propBlock = new MaterialPropertyBlock();
            propBlock.SetColor("_Color", hitColor);
            propBlock.SetColor("_BaseColor", hitColor);
            propBlock.SetColor("_EmissionColor", hitColor * 0.2f);
            
            renderer.SetPropertyBlock(propBlock);
        }
    }
    
    private void SetColorToOriginal()
    {
        foreach (var renderer in playerRenderers)
        {
            if (renderer == null) continue;
            
            // Просто очищаем property block чтобы вернуть оригинальные значения
            renderer.SetPropertyBlock(null);
        }
    }
    
    [ContextMenu("Auto Find Renderers")]
    public void AutoFindRenderers()
    {
        playerRenderers.Clear();
        var renderers = GetComponentsInChildren<Renderer>(true);
        playerRenderers.AddRange(renderers);
        Debug.Log($"Found {playerRenderers.Count} renderers");
    }
}