using UnityEngine;
using System.Collections;

public class EnemyHitEffect : MonoBehaviour
{
    [Header("Effect Settings")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.1f;
    
    private Renderer[] renderers;
    private Coroutine flashCoroutine;
    
    void Awake()
    {
        // Кэшируем все рендеры при инициализации
        renderers = GetComponentsInChildren<Renderer>();
        
        // Отключаем ненужные компоненты если объект не виден
        if (renderers.Length == 0)
            enabled = false;
    }
    
    public void TakeHit()
    {
        // Останавливаем предыдущую корутину если она есть
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashOnce());
       

    }
    
    private IEnumerator FlashOnce()
    {
            
        // Включаем красный цвет
        SetColorToRed();
        
            // Ждем указанное время
            yield return new WaitForSeconds(flashDuration);

            // Возвращаем оригинальный цвет
            SetColorToOriginal();
        
       
    }
    
    private void SetColorToRed()
    {
        foreach (var renderer in renderers)
        {
            if (renderer == null) continue;
            
            var propBlock = new MaterialPropertyBlock();
            propBlock.SetColor("_Color", hitColor);
            propBlock.SetColor("_BaseColor", hitColor);
            
            renderer.SetPropertyBlock(propBlock);
        }
    }
    
    private void SetColorToOriginal()
    {
        foreach (var renderer in renderers)
        {
            if (renderer == null) continue;
            renderer.SetPropertyBlock(null);
        }
    }
    
    void OnDisable()
    {
        // Гарантируем сброс цвета при деактивации
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            SetColorToOriginal();
        }
    }
}