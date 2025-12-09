using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioSource walkSource;
    public AudioSource hitSource;
    public AudioSource shootSource;
    public AudioSource openChestSource;
    public AudioSource QuestSource;
    public AudioSource levelUpSource;
    public AudioSource uiClickSource;

    [Header("Audio Mixer (опционально)")]
    public AudioMixer audioMixer;

    [Header("Громкость по умолчанию")]
    [Range(0f, 1f)]
    public float masterVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    // Для сохранения настроек
    private const string MASTER_VOL_KEY = "MasterVolume";
    private const string SFX_VOL_KEY = "SFXVolume";
    private const string MUSIC_VOL_KEY = "MusicVolume";

    private bool isMusicPaused = false;

    void Awake()
    {
        // Singleton паттерн
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Инициализация источников звука
        InitializeAudioSources();

        // Загрузка сохраненных настроек
        LoadVolumeSettings();
    }

    void InitializeAudioSources()
    {
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        if (uiClickSource == null)
        {
            uiClickSource = gameObject.AddComponent<AudioSource>();
            uiClickSource.playOnAwake = false;
            uiClickSource.loop = false;
        }

        if (walkSource == null)
        {
            walkSource = gameObject.AddComponent<AudioSource>();
            walkSource.playOnAwake = false;
            walkSource.loop = true;
        }

        if (hitSource == null)
        {
            hitSource = gameObject.AddComponent<AudioSource>();
            hitSource.playOnAwake = false;
            hitSource.loop = false;
        }

        if (shootSource == null)
        {
            shootSource = gameObject.AddComponent<AudioSource>();
            shootSource.playOnAwake = false;
            shootSource.loop = false;
        }

        if (openChestSource == null)
        {
            openChestSource = gameObject.AddComponent<AudioSource>();
            openChestSource.playOnAwake = false;
            openChestSource.loop = false;
        }

        if (QuestSource == null)
        {
            QuestSource = gameObject.AddComponent<AudioSource>();
            QuestSource.playOnAwake = false;
            QuestSource.loop = false;
        }

        if (levelUpSource == null)
        {
            levelUpSource = gameObject.AddComponent<AudioSource>();
            levelUpSource.playOnAwake = false;
            levelUpSource.loop = false;
        }

    }

    // === МЕТОДЫ ВОСПРОИЗВЕДЕНИЯ ===

    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
    }

    public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volumeScale = 1f)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, position, volumeScale * sfxVolume * masterVolume);
    }

    public void PlayWalk(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null && !walkSource.isPlaying)
        {
            walkSource.clip = clip;
            walkSource.volume = volumeScale * sfxVolume * masterVolume;
            walkSource.Play();
        }
    }

    public void StopWalk()
    {
        if (walkSource.isPlaying)
            walkSource.Stop();
    }

    public void PlayHit(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
        {
            // Всегда играем, даже если звук уже играет (перезаписываем)
            hitSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
        }
    }

    public void StopHit()
    {
        hitSource.Stop();
    }

    public void PlayShoot(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
        {
            // Всегда играем, даже если звук уже играет (перезаписываем)
            shootSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
        }
    }

    public void StopShoot()
    {
        shootSource.Stop();
    }

    public void PlayOpenChest(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
        {
            // Всегда играем, даже если звук уже играет (перезаписываем)
            openChestSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
        }
    }

    public void PlayQuest(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
        {
            // Всегда играем, даже если звук уже играет (перезаписываем)
            QuestSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
        }
    }

    public void PlayLevelUp(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
        {
            // Всегда играем, даже если звук уже играет (перезаписываем)
            levelUpSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
        }
    }



    public void PlayClick(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
        {
            // Всегда играем, даже если звук уже играет (перезаписываем)
            uiClickSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
        }
    }

    public void PauseMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Pause();
            isMusicPaused = true;
            Debug.Log("Music paused");
        }
    }

    public void ResumeMusic()
    {
        if (musicSource != null && isMusicPaused)
        {
            musicSource.UnPause();
            isMusicPaused = false;
            Debug.Log("Music resumed");
        }
        else if (musicSource != null && !musicSource.isPlaying)
        {
            // Если музыка не играла (например, только запустили игру)
            musicSource.Play();
            Debug.Log("Music started (wasn't playing)");
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            isMusicPaused = false;
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true, float volumeScale = 1f)
    {
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = volumeScale * musicVolume * masterVolume;
            musicSource.Play();
            isMusicPaused = false;
        }
    }

    public void DuckMusic(float duckAmount = 0.5f, float fadeDuration = 0.5f)
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            StartCoroutine(DuckMusicCoroutine(duckAmount, fadeDuration));
        }
    }

    public void UnduckMusic(float fadeDuration = 0.5f)
    {
        if (musicSource != null)
        {
            StartCoroutine(UnduckMusicCoroutine(fadeDuration));
        }
    }

    private IEnumerator DuckMusicCoroutine(float targetDuckAmount, float duration)
    {
        float startVolume = musicSource.volume;
        float targetVolume = startVolume * targetDuckAmount;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Используем unscaled чтобы работало на паузе
            float t = elapsedTime / duration;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    private IEnumerator UnduckMusicCoroutine(float duration)
    {
        float startVolume = musicSource.volume;
        float targetVolume = musicSource.volume / (musicSource.volume / (musicVolume * masterVolume));
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / duration;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    public void SetMusicPitch(float pitch, float transitionDuration = 1f)
    {
        if(pitch < 0.5f) pitch = 0.5f;
        if (musicSource != null)
        {
            StartCoroutine(ChangePitchCoroutine(pitch, transitionDuration));
        }
    }

    public void ResetMusicPitch(float transitionDuration = 1f)
    {
        SetMusicPitch(1f, transitionDuration);
    }

    private IEnumerator ChangePitchCoroutine(float targetPitch, float duration)
    {
        float startPitch = musicSource.pitch;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / duration;
            musicSource.pitch = Mathf.Lerp(startPitch, targetPitch, t);
            yield return null;
        }

        musicSource.pitch = targetPitch;
    }

    // === НАСТРОЙКА ГРОМКОСТИ ===

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
        SaveVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateSFXVolume();
        SaveVolumeSettings();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateMusicVolume();
        SaveVolumeSettings();
    }

    private void UpdateAllVolumes()
    {
        UpdateSFXVolume();
        UpdateMusicVolume();

        if (uiClickSource != null)
            uiClickSource.volume = QuestSource.volume;
    }

    private void UpdateSFXVolume()
    {
        if (sfxSource != null)
            sfxSource.volume = sfxVolume * masterVolume;

        if (walkSource != null)
            walkSource.volume = walkSource.volume;

        if (hitSource != null)
            hitSource.volume = hitSource.volume;

        if (shootSource != null)
            shootSource.volume = shootSource.volume;

        if (openChestSource != null)
            openChestSource.volume = openChestSource.volume;

        if (QuestSource != null)
            QuestSource.volume = QuestSource.volume;

        if (levelUpSource != null)
            levelUpSource.volume = QuestSource.volume;
    }

    private void UpdateMusicVolume()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume * masterVolume;
    }

    // === РАБОТА С AUDIO MIXER ===

    public void SetMasterVolumeMixer(float volume)
    {
        if (audioMixer != null)
        {
            // Конвертируем линейное значение (0-1) в децибелы (-80 до 0)
            float dbVolume = volume > 0.0001f ? 20f * Mathf.Log10(volume) : -80f;
            audioMixer.SetFloat("MasterVolume", dbVolume);
        }
        else
        {
            SetMasterVolume(volume);
        }
    }

    public void SetSFXVolumeMixer(float volume)
    {
        if (audioMixer != null)
        {
            float dbVolume = volume > 0.0001f ? 20f * Mathf.Log10(volume) : -80f;
            audioMixer.SetFloat("SFXVolume", dbVolume);
        }
        else
        {
            SetSFXVolume(volume);
        }
    }

    public void SetMusicVolumeMixer(float volume)
    {
        if (audioMixer != null)
        {
            float dbVolume = volume > 0.0001f ? 20f * Mathf.Log10(volume) : -80f;
            audioMixer.SetFloat("MusicVolume", dbVolume);
        }
        else
        {
            SetMusicVolume(volume);
        }
    }

    // === СОХРАНЕНИЕ НАСТРОЕК ===

    private void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat(MASTER_VOL_KEY, masterVolume);
        PlayerPrefs.SetFloat(SFX_VOL_KEY, sfxVolume);
        PlayerPrefs.SetFloat(MUSIC_VOL_KEY, musicVolume);
        PlayerPrefs.Save();
    }

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey(MASTER_VOL_KEY))
            masterVolume = PlayerPrefs.GetFloat(MASTER_VOL_KEY);

        if (PlayerPrefs.HasKey(SFX_VOL_KEY))
            sfxVolume = PlayerPrefs.GetFloat(SFX_VOL_KEY);

        if (PlayerPrefs.HasKey(MUSIC_VOL_KEY))
            musicVolume = PlayerPrefs.GetFloat(MUSIC_VOL_KEY);

        UpdateAllVolumes();
    }

    // === ДОПОЛНИТЕЛЬНЫЕ ФУНКЦИИ ===

    public void ToggleMute()
    {
        float targetVolume = (masterVolume > 0f) ? 0f : 1f;
        SetMasterVolume(targetVolume);
    }

    public void ToggleMusicMute()
    {
        float targetVolume = (musicVolume > 0f) ? 0f : 1f;
        SetMusicVolume(targetVolume);
    }

    public void ToggleSFXMute()
    {
        float targetVolume = (sfxVolume > 0f) ? 0f : 1f;
        SetSFXVolume(targetVolume);
    }

    // Для UI слайдеров
    public float GetMasterVolume() => masterVolume;
    public float GetSFXVolume() => sfxVolume;
    public float GetMusicVolume() => musicVolume;

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}