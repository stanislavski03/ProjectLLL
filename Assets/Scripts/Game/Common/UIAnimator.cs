using UnityEngine;
using DG.Tweening;

public class UIAnimator : MonoBehaviour
{
    public enum AnimationType
    {
        ScaleUp,        // Увеличение
        SlideFromLeft,  // Слайд слева
        SlideFromRight, // Слайд справа
        SlideFromBottom,// Слайд снизу
        SlideFromTop,   // Слайд сверху
        FadeIn,         // Появление
        Bounce,         // Отскок
        Flip,           // Переворот
        Pulse,          // Пульсация
        Shake           // Тряска
    }

    [Header("Настройки анимации")]
    public AnimationType animationType = AnimationType.ScaleUp;
    public float duration = 0.5f;
    public float delay = 0f;
    public Ease easeType = Ease.OutBack;
    
    [Header("Параметры анимации")]
    public float strength = 1.2f; // Для ScaleUp, Bounce
    public Vector3 slideOffset = new Vector3(100, 0, 0); // Для слайдов
    
    [Header("Автоматическое выполнение")]
    public bool animateOnEnable = true;
    public bool animateOnStart = false;
    public bool resetOnDisable = true;
    
    [Header("Настройки времени")]
    public bool ignoreTimescale = true; // Работает даже при Time.timeScale = 0

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private CanvasGroup canvasGroup;
    private Tween currentTween;

    void Start()
    {
        if (animateOnStart && !animateOnEnable)
        {
            AnimateIn();
        }
    }

    void OnEnable()
    {
        if (animateOnEnable)
        {
            AnimateIn();
        }
    }

    void OnDisable()
    {
        if (resetOnDisable)
        {
            ResetToOriginal();
        }
        
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
    }

    void ResetToOriginal()
    {
        transform.localPosition = originalPosition;
        transform.localScale = originalScale;
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
    }

    public void AnimateIn()
    {
        // Сохраняем оригинальные значения
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;
        
        // Получаем или создаем CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null && (animationType == AnimationType.FadeIn || animationType == AnimationType.Pulse))
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Сбрасываем перед анимацией
        ResetToOriginal();
        
        // Запускаем выбранную анимацию
        switch (animationType)
        {
            case AnimationType.ScaleUp:
                AnimateScaleUp();
                break;
                
            case AnimationType.SlideFromLeft:
                AnimateSlide(Vector3.left);
                break;
                
            case AnimationType.SlideFromRight:
                AnimateSlide(Vector3.right);
                break;
                
            case AnimationType.SlideFromBottom:
                AnimateSlide(Vector3.down);
                break;
                
            case AnimationType.SlideFromTop:
                AnimateSlide(Vector3.up);
                break;
                
            case AnimationType.FadeIn:
                AnimateFadeIn();
                break;
                
            case AnimationType.Bounce:
                AnimateBounce();
                break;
                
            case AnimationType.Flip:
                AnimateFlip();
                break;
                
            case AnimationType.Pulse:
                AnimatePulse();
                break;
                
            case AnimationType.Shake:
                AnimateShake();
                break;
        }
    }

    public void AnimateOut()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        TweenParams tweenParams = new TweenParams()
            .SetDelay(delay)
            .SetEase(easeType);
            
        if (ignoreTimescale)
        {
            tweenParams.SetUpdate(UpdateType.Normal, true);
        }

        switch (animationType)
        {
            case AnimationType.ScaleUp:
                currentTween = transform.DOScale(0, duration)
                    .SetAs(tweenParams);
                break;
                
            case AnimationType.FadeIn:
                if (canvasGroup != null)
                {
                    currentTween = canvasGroup.DOFade(0, duration)
                        .SetAs(tweenParams);
                }
                break;
                
            default:
                if (canvasGroup == null) 
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                    
                currentTween = canvasGroup.DOFade(0, duration)
                    .SetAs(tweenParams)
                    .OnComplete(() => gameObject.SetActive(false));
                break;
        }
    }

    void AnimateScaleUp()
    {
        transform.localScale = Vector3.zero;
        
        currentTween = transform.DOScale(originalScale * strength, duration * 0.7f)
            .SetDelay(delay)
            .SetEase(easeType)
            .SetUpdate(ignoreTimescale)
            .OnComplete(() => {
                transform.DOScale(originalScale, duration * 0.3f)
                    .SetEase(Ease.OutQuad)
                    .SetUpdate(ignoreTimescale);
            });
    }

    void AnimateSlide(Vector3 direction)
    {
        Vector3 startPos = originalPosition + new Vector3(
            slideOffset.x * direction.x,
            slideOffset.y * direction.y,
            0
        );
        
        transform.localPosition = startPos;
        
        currentTween = transform.DOLocalMove(originalPosition, duration)
            .SetDelay(delay)
            .SetEase(easeType)
            .SetUpdate(ignoreTimescale);
    }

    void AnimateFadeIn()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            currentTween = canvasGroup.DOFade(1, duration)
                .SetDelay(delay)
                .SetEase(easeType)
                .SetUpdate(ignoreTimescale);
        }
    }

    void AnimateBounce()
    {
        transform.localScale = Vector3.zero;
        
        currentTween = transform.DOScale(originalScale * strength, duration)
            .SetDelay(delay)
            .SetEase(Ease.OutBounce)
            .SetUpdate(ignoreTimescale)
            .OnComplete(() => {
                transform.DOScale(originalScale, duration * 0.3f)
                    .SetUpdate(ignoreTimescale);
            });
    }

    void AnimateFlip()
    {
        transform.localScale = new Vector3(0, originalScale.y, originalScale.z);
        
        currentTween = transform.DOScaleX(originalScale.x, duration)
            .SetDelay(delay)
            .SetEase(easeType)
            .SetUpdate(ignoreTimescale);
    }

    void AnimatePulse()
    {
        Sequence pulseSequence = DOTween.Sequence();
        
        pulseSequence.Append(transform.DOScale(originalScale * 1.1f, duration * 0.3f));
        pulseSequence.Append(transform.DOScale(originalScale * 0.9f, duration * 0.2f));
        pulseSequence.Append(transform.DOScale(originalScale * 1.05f, duration * 0.15f));
        pulseSequence.Append(transform.DOScale(originalScale, duration * 0.15f));
        pulseSequence.SetDelay(delay);
        
        if (ignoreTimescale)
        {
            pulseSequence.SetUpdate(UpdateType.Normal, true);
        }
        
        currentTween = pulseSequence;
    }

    void AnimateShake()
    {
        currentTween = transform.DOShakePosition(duration, strength: 10f, vibrato: 20)
            .SetDelay(delay)
            .SetUpdate(ignoreTimescale);
    }

    // Метод для анимации с кастомными параметрами
    public void AnimateInCustom(float customDuration, float customDelay, Ease customEase)
    {
        duration = customDuration;
        delay = customDelay;
        easeType = customEase;
        AnimateIn();
    }

    // Метод для вызова из других скриптов
    public void PlayAnimation(AnimationType type)
    {
        animationType = type;
        AnimateIn();
    }

    // Метод для остановки анимации
    public void StopAnimation()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        ResetToOriginal();
    }

    // Метод для паузы/возобновления анимации
    public void PauseAnimation(bool pause)
    {
        if (currentTween != null && currentTween.IsActive())
        {
            if (pause)
                currentTween.Pause();
            else
                currentTween.Play();
        }
    }

    // Быстрые методы для кнопок и событий
    public void PlayScaleUp() => PlayAnimation(AnimationType.ScaleUp);
    public void PlaySlideFromLeft() => PlayAnimation(AnimationType.SlideFromLeft);
    public void PlaySlideFromBottom() => PlayAnimation(AnimationType.SlideFromBottom);
    public void PlayFadeIn() => PlayAnimation(AnimationType.FadeIn);
    public void PlayBounce() => PlayAnimation(AnimationType.Bounce);
    
    // Готовые пресеты
    public void PlayQuickScaleUp()
    {
        animationType = AnimationType.ScaleUp;
        duration = 0.3f;
        strength = 1.5f;
        AnimateIn();
    }
    
    public void PlaySmoothFade()
    {
        animationType = AnimationType.FadeIn;
        duration = 0.8f;
        easeType = Ease.InOutSine;
        AnimateIn();
    }
    
    public void PlayAttentionBounce()
    {
        animationType = AnimationType.Bounce;
        duration = 0.6f;
        strength = 1.3f;
        AnimateIn();
    }
}