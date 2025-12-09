using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleFog : MonoBehaviour
{
    public static SimpleFog Instance { get; private set; }
    
    [Header("Fog Settings")]
    public float defaultFogDensity = 0.01f;
    public float pauseFogDensity = 0.1f;
    public float transitionDuration = 0.5f;
    public Color fogColor = Color.gray;
    
    private Coroutine fogTransitionCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Отписываемся при уничтожении объекта
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Принудительно применяем настройки тумана при загрузке новой сцены
        ApplyFogSettings();
    }

    void Start()
    {
        ApplyFogSettings();
    }

    private void ApplyFogSettings()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = defaultFogDensity;
    }

    public void SetFogSettings(float intensity)
    {
        RenderSettings.fogDensity = intensity;
    }
    
    public void SetFogSmooth(float targetIntensity)
    {
        if (fogTransitionCoroutine != null)
            StopCoroutine(fogTransitionCoroutine);
            
        fogTransitionCoroutine = StartCoroutine(SmoothFogTransition(targetIntensity));
    }
    
    private IEnumerator SmoothFogTransition(float targetIntensity)
    {
        float startIntensity = RenderSettings.fogDensity;
        float elapsedTime = 0f;
        
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / transitionDuration;
            RenderSettings.fogDensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            yield return null;
        }
        
        RenderSettings.fogDensity = targetIntensity;
    }
}