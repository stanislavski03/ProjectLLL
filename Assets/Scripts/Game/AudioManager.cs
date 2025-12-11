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
        // Создаем и настраиваем источники звука
        AudioSource[] sources = new AudioSource[] {
            sfxSource, musicSource, uiClickSource, walkSource,
            hitSource, shootSource, openChestSource, QuestSource, levelUpSource
        };

        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i] == null)
            {
                sources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        // Сохраняем ссылки
        sfxSource = sources[0];
        musicSource = sources[1];
        uiClickSource = sources[2];
        walkSource = sources[3];
        hitSource = sources[4];
        shootSource = sources[5];
        openChestSource = sources[6];
        QuestSource = sources[7];
        levelUpSource = sources[8];

        // Настройка свойств
        musicSource.loop = true;
        walkSource.loop = true;

        // Устанавливаем всем playOnAwake = false
        foreach (var source in sources)
        {
            source.playOnAwake = false;
            sourceBaseVolumes[source] = 1f; // Базовая громкость 1
        }
    }

    // === МЕТОДЫ ВОСПРОИЗВЕДЕНИЯ ===

    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
        }
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
            sourceBaseVolumes[walkSource] = volumeScale;
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
            sourceBaseVolumes[hitSource] = volumeScale;
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
            sourceBaseVolumes[shootSource] = volumeScale;
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
            sourceBaseVolumes[openChestSource] = volumeScale;
            openChestSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
        }
    }

    public void PlayQuest(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
        {
            sourceBaseVolumes[QuestSource] = volumeScale;
            QuestSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
        }
    }

    public void PlayLevelUp(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
        {
            sourceBaseVolumes[levelUpSource] = volumeScale;
            levelUpSource.PlayOneShot(clip, volumeScale * sfxVolume * masterVolume);
        }
    }

    public void PlayClick(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
        {
            sourceBaseVolumes[uiClickSource] = volumeScale;
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
            sourceBaseVolumes[musicSource] = volumeScale;
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
        float targetVolume = baseVolume * musicVolume * masterVolume;
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
        UpdateAllVolumes(); // Обновляем все объемы, включая SFX
        SaveVolumeSettings();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes(); // Обновляем все объемы, включая музыку
        SaveVolumeSettings();
    }

    private void UpdateAllVolumes()
    {
        // Обновляем все активные источники звука немедленно
        UpdateSourceVolume(sfxSource, sfxVolume * masterVolume);
        UpdateSourceVolume(musicSource, musicVolume * masterVolume);
        UpdateSourceVolume(uiClickSource, sfxVolume * masterVolume);
        UpdateSourceVolume(walkSource, sfxVolume * masterVolume);
        UpdateSourceVolume(hitSource, sfxVolume * masterVolume);
        UpdateSourceVolume(shootSource, sfxVolume * masterVolume);
        UpdateSourceVolume(openChestSource, sfxVolume * masterVolume);
        UpdateSourceVolume(QuestSource, sfxVolume * masterVolume);
        UpdateSourceVolume(levelUpSource, sfxVolume * masterVolume);
    }

    private void UpdateSourceVolume(AudioSource source, float globalVolumeMultiplier)
    {
        if (source != null && sourceBaseVolumes.ContainsKey(source))
        {
            float baseVolume = sourceBaseVolumes[source];
            source.volume = baseVolume * globalVolumeMultiplier;
        }
    }

    // === РАБОТА С AUDIO MIXER ===

    public void SetMasterVolumeMixer(float volume)
    {
        if (audioMixer != null)
        {
            // Конвертируем линейное значение (0-1) в децибелы (-80 до 0)
            float dbVolume = volume > 0.0001f ? 20f * Mathf.Log10(volume) : -80f;
            audioMixer.SetFloat("Master", dbVolume);
            SetMasterVolume(volume);
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
            SetSFXVolume(volume);
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
            SetMusicVolume(volume);
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