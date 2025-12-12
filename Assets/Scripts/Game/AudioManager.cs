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
    private const string MASTER_VOL_KEY = "Master";
    private const string SFX_VOL_KEY = "SFXVolume";
    private const string MUSIC_VOL_KEY = "MusicVolume";

    private bool isMusicPaused = false;
    private Dictionary<AudioSource, float> sourceBaseVolumes = new Dictionary<AudioSource, float>();

    void Awake()
    {
        // Singleton паттерн
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Настройка источников звука
        ConfigureAudioSources();

        // Загрузка сохраненных настроек
        LoadVolumeSettings();
    }

    void ConfigureAudioSources()
    {
        // Настройка свойств
        if (musicSource != null)
            musicSource.loop = true;
        
        if (walkSource != null)
            walkSource.loop = true;

        // Устанавливаем всем playOnAwake = false и сохраняем начальные громкости
        AudioSource[] sources = new AudioSource[] {
            sfxSource, musicSource, uiClickSource, walkSource,
            hitSource, shootSource, openChestSource, QuestSource, levelUpSource
        };

        foreach (var source in sources)
        {
            if (source != null)
            {
                source.playOnAwake = false;
                // Сохраняем текущую громкость как базовую (по умолчанию 1)
                if (!sourceBaseVolumes.ContainsKey(source))
                {
                    sourceBaseVolumes[source] = 1f;
                }
            }
        }

        // Применяем текущие настройки громкости
        UpdateAllVolumes();
    }

    // === МЕТОДЫ ВОСПРОИЗВЕДЕНИЯ ===

    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null && sfxSource != null)
        {
            // Используем только volumeScale, громкость регулируется через микшер
            sfxSource.PlayOneShot(clip, volumeScale);
        }
    }

    public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volumeScale = 1f)
    {
        if (clip != null)
        {
            // Используем только volumeScale, громкость регулируется через микшер
            AudioSource.PlayClipAtPoint(clip, position, volumeScale);
        }
    }

    public void PlayWalk(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null && walkSource != null && !walkSource.isPlaying)
        {
            walkSource.clip = clip;
            sourceBaseVolumes[walkSource] = volumeScale;
            walkSource.volume = volumeScale;
            walkSource.Play();
        }
    }

    public void StopAllSFX()
    {
        StopWalk();
        StopHit();
        StopShoot();
    }

    public void StopWalk()
    {
        if (walkSource != null && walkSource.isPlaying)
            walkSource.Stop();
    }

    public void PlayHit(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null && hitSource != null)
        {
            sourceBaseVolumes[hitSource] = volumeScale;
            hitSource.PlayOneShot(clip, volumeScale);
        }
    }

    public void StopHit()
    {
        if (hitSource != null)
            hitSource.Stop();
    }

    public void PlayShoot(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null && shootSource != null)
        {
            sourceBaseVolumes[shootSource] = volumeScale;
            shootSource.PlayOneShot(clip, volumeScale);
        }
    }

    public void StopShoot()
    {
        if (shootSource != null)
            shootSource.Stop();
    }

    public void PlayOpenChest(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null && openChestSource != null)
        {
            sourceBaseVolumes[openChestSource] = volumeScale;
            openChestSource.PlayOneShot(clip, volumeScale);
        }
    }

    public void PlayQuest(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null && QuestSource != null)
        {
            sourceBaseVolumes[QuestSource] = volumeScale;
            QuestSource.PlayOneShot(clip, volumeScale);
        }
    }

    public void PlayLevelUp(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null && levelUpSource != null)
        {
            sourceBaseVolumes[levelUpSource] = volumeScale;
            levelUpSource.PlayOneShot(clip, volumeScale);
        }
    }

    public void PlayClick(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null && uiClickSource != null)
        {
            sourceBaseVolumes[uiClickSource] = volumeScale;
            uiClickSource.PlayOneShot(clip, volumeScale);
        }
    }

    // === МУЗЫКА ===

    public void PauseMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Pause();
            isMusicPaused = true;
        }
    }

    public void ResumeMusic()
    {
        if (musicSource != null)
        {
            if (isMusicPaused)
            {
                musicSource.UnPause();
                isMusicPaused = false;
            }
            else if (!musicSource.isPlaying && musicSource.clip != null)
            {
                musicSource.Play();
            }
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
        if (clip != null && musicSource != null)
        {
            sourceBaseVolumes[musicSource] = volumeScale;
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = volumeScale;
            musicSource.Play();
            isMusicPaused = false;
        }
    }

    // === DUCKING И PITCH ===

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
        float baseVolume = sourceBaseVolumes.ContainsKey(musicSource) ? sourceBaseVolumes[musicSource] : 1f;
        float targetVolume = baseVolume * targetDuckAmount;
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

    private IEnumerator UnduckMusicCoroutine(float duration)
    {
        float startVolume = musicSource.volume;
        float baseVolume = sourceBaseVolumes.ContainsKey(musicSource) ? sourceBaseVolumes[musicSource] : 1f;
        float targetVolume = baseVolume;
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
        if (pitch < 0.5f) pitch = 0.5f;
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
            try
            {
                elapsedTime += Time.unscaledDeltaTime;
                float t = elapsedTime / duration;
                musicSource.pitch = Mathf.Lerp(startPitch, targetPitch, t);
                yield return null;
            }
            finally { }
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
        UpdateAllVolumes();
        SaveVolumeSettings();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
        SaveVolumeSettings();
    }

    private void UpdateAllVolumes()
    {
        // Обновляем громкость только через микшер
        UpdateMixerVolumes();
    }

    private void UpdateMixerVolumes()
    {
        if (audioMixer != null)
        {
            // Конвертируем линейные значения (0-1) в децибелы (-80 до 0)
            float masterDb = masterVolume > 0.0001f ? 20f * Mathf.Log10(masterVolume) : -80f;
            float sfxDb = sfxVolume > 0.0001f ? 20f * Mathf.Log10(sfxVolume) : -80f;
            float musicDb = musicVolume > 0.0001f ? 20f * Mathf.Log10(musicVolume) : -80f;

            audioMixer.SetFloat("Master", masterDb);
            audioMixer.SetFloat("SFXVolume", sfxDb);
            audioMixer.SetFloat("MusicVolume", musicDb);
        }
    }

    // === РАБОТА С AUDIO MIXER ===

    public void SetMasterVolumeMixer(float volume)
    {
        SetMasterVolume(volume);
    }

    public void SetSFXVolumeMixer(float volume)
    {
        SetSFXVolume(volume);
    }

    public void SetMusicVolumeMixer(float volume)
    {
        SetMusicVolume(volume);
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
        
        // Применяем загруженные настройки к микшеру
        UpdateMixerVolumes();
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