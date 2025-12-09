using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FinalVignette : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private PlayerHP playerHP;

    [Header("Настройки виньетки по здоровью")]
    [SerializeField] private Color healthVignetteColor = Color.red;
    [SerializeField] private float maxHealthIntensity = 0.6f;
    [SerializeField] private float healthSmoothTime = 0.3f;
    [SerializeField] private float healthThreshold = 0.5f;

    [Header("Настройки виньетки при уроне")]
    [SerializeField] private Color damageVignetteColor = new Color(1f, 0.3f, 0.3f);
    [SerializeField] private float damageIntensity = 0.8f;
    [SerializeField] private float damageDuration = 0.4f;
    [SerializeField] private float damageFadeInTime = 0.05f;
    [SerializeField] private float damageFadeOutTime = 0.35f;

    [Header("Настройки виньетки при исцелении")]
    [SerializeField] private Color healVignetteColor = new Color(0.3f, 1f, 0.3f); // Зеленый
    [SerializeField] private float healIntensity = 0.5f;
    [SerializeField] private float healDuration = 0.3f;
    [SerializeField] private float healFadeInTime = 0.1f;
    [SerializeField] private float healFadeOutTime = 0.2f;

    [Header("Общие настройки")]
    [SerializeField] private float smoothness = 0.35f;

    private Volume volume;
    private Vignette vignette;

    // Для виньетки здоровья
    private float healthTargetIntensity = 0f;
    private float healthCurrentIntensity = 0f;
    private float healthVelocity = 0f;

    // Для виньетки урона
    private float damageCurrentIntensity = 0f;
    private bool isDamageEffectActive = false;
    private float damageTimer = 0f;

    // Для виньетки исцеления
    private float healCurrentIntensity = 0f;
    private bool isHealEffectActive = false;
    private float healTimer = 0f;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        SetupVolume();
        SetupPlayer();
    }

    void SetupVolume()
    {
        volume = FindObjectOfType<Volume>();
        if (volume == null)
        {
            GameObject volumeGO = new GameObject("FinalVignetteVolume");
            volume = volumeGO.AddComponent<Volume>();
            volume.isGlobal = true;
            volume.weight = 1f;
        }

        if (volume.profile == null)
        {
            volume.profile = ScriptableObject.CreateInstance<VolumeProfile>();
        }

        if (!volume.profile.TryGet(out vignette))
        {
            vignette = volume.profile.Add<Vignette>(true);
            vignette.intensity.Override(0f);
            vignette.color.Override(healthVignetteColor);
            vignette.smoothness.Override(smoothness);
            vignette.rounded.Override(false);
        }
    }

    void SetupPlayer()
    {
        if (playerHP == null)
        {
            playerHP = FindObjectOfType<PlayerHP>();
        }

        if (playerHP != null)
        {
            playerHP.Changed += OnHealthChanged;
            OnHealthChanged(playerHP._currentHP);
        }
    }

    void OnHealthChanged(float currentHealth)
    {
        if (playerHP == null) return;

        float maxHealth = playerHP.MaxHP;
        if (maxHealth <= 0) return;

        float healthPercentage = currentHealth / maxHealth;

        if (healthPercentage <= healthThreshold)
        {
            float normalized = Mathf.Pow(1f - (healthPercentage / healthThreshold), 0.7f);
            healthTargetIntensity = normalized * maxHealthIntensity;
        }
        else
        {
            healthTargetIntensity = 0f;
        }
    }

    // Урон
    public void OnDamageTaken()
    {
        if (!isDamageEffectActive)
        {
            isDamageEffectActive = true;
            damageTimer = 0f;
        }
        else
        {
            damageTimer = 0f;
        }
    }

    // Исцеление
    public void OnHealApplied()
    {
        if (!isHealEffectActive)
        {
            isHealEffectActive = true;
            healTimer = 0f;
        }
        else
        {
            healTimer = 0f;
        }
    }

    void Update()
    {
        if (vignette == null) return;

        if (playerHP == null)
        {
            playerHP = FindObjectOfType<PlayerHP>();
            if (playerHP != null)
            {
                playerHP.Changed += OnHealthChanged;
                OnHealthChanged(playerHP._currentHP);
            }
        }

        // 1. Виньетка здоровья
        healthCurrentIntensity = Mathf.SmoothDamp(
            healthCurrentIntensity,
            healthTargetIntensity,
            ref healthVelocity,
            healthSmoothTime
        );

        // 2. Виньетка урона
        if (isDamageEffectActive)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer <= damageDuration)
            {
                if (damageTimer <= damageFadeInTime)
                {
                    float t = damageTimer / damageFadeInTime;
                    damageCurrentIntensity = Mathf.Lerp(0f, damageIntensity, t);
                }
                else if (damageTimer >= damageDuration - damageFadeOutTime)
                {
                    float t = (damageTimer - (damageDuration - damageFadeOutTime)) / damageFadeOutTime;
                    damageCurrentIntensity = Mathf.Lerp(damageIntensity, 0f, t);
                }
                else
                {
                    damageCurrentIntensity = damageIntensity;
                }
            }
            else
            {
                isDamageEffectActive = false;
                damageCurrentIntensity = 0f;
            }
        }
        else
        {
            damageCurrentIntensity = 0f;
        }

        // 3. Виньетка исцеления
        if (isHealEffectActive)
        {
            healTimer += Time.deltaTime;

            if (healTimer <= healDuration)
            {
                if (healTimer <= healFadeInTime)
                {
                    float t = healTimer / healFadeInTime;
                    healCurrentIntensity = Mathf.Lerp(0f, healIntensity, t);
                }
                else if (healTimer >= healDuration - healFadeOutTime)
                {
                    float t = (healTimer - (healDuration - healFadeOutTime)) / healFadeOutTime;
                    healCurrentIntensity = Mathf.Lerp(healIntensity, 0f, t);
                }
                else
                {
                    healCurrentIntensity = healIntensity;
                }
            }
            else
            {
                isHealEffectActive = false;
                healCurrentIntensity = 0f;
            }
        }
        else
        {
            healCurrentIntensity = 0f;
        }

        // 4. Комбинируем все эффекты
        float combinedIntensity = Mathf.Max(healthCurrentIntensity, damageCurrentIntensity, healCurrentIntensity);
        vignette.intensity.value = combinedIntensity;

        // 5. Выбираем цвет по приоритету: исцеление > урон > здоровье
        if (healCurrentIntensity > Mathf.Max(healthCurrentIntensity, damageCurrentIntensity))
        {
            vignette.color.value = healVignetteColor;
        }
        else if (damageCurrentIntensity > healthCurrentIntensity)
        {
            vignette.color.value = damageVignetteColor;
        }
        else if (healthCurrentIntensity > 0f)
        {
            vignette.color.value = healthVignetteColor;
        }
        else
        {
            vignette.color.value = healthVignetteColor;
        }
    }

    // Публичные методы для вызова из других скриптов
    public void TriggerDamageVignette()
    {
        OnDamageTaken();
    }

    public void TriggerHealVignette()
    {
        OnHealApplied();
    }

    public void SetHealthVignette(float intensity)
    {
        healthTargetIntensity = Mathf.Clamp01(intensity) * maxHealthIntensity;
    }

    void OnDestroy()
    {
        if (playerHP != null)
        {
            playerHP.Changed -= OnHealthChanged;
        }

        if (vignette != null)
        {
            vignette.intensity.value = 0f;
        }
    }
}