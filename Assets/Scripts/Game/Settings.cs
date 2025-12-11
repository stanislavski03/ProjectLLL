using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Settings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    Resolution[] resolutions;

    void Start()
    {
        // Инициализация выпадающих списков
        resolutionDropdown.ClearOptions();
        qualityDropdown.ClearOptions();
        
        // Заполняем список разрешений
        resolutions = Screen.resolutions;
        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRateRatio + "Hz";
            resolutionOptions.Add(option);
            
            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(resolutionOptions);
        
        // Заполняем список качеств графики
        List<string> qualityOptions = new List<string>();
        string[] qualityNames = QualitySettings.names;
        
        for (int i = 0; i < qualityNames.Length; i++)
        {
            qualityOptions.Add(qualityNames[i]);
        }
        
        qualityDropdown.AddOptions(qualityOptions);
        
        // Обновляем отображаемые значения
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.RefreshShownValue();
        
        // Загружаем сохраненные настройки
        LoadSettings(currentResolutionIndex);
    }
    
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.Save();
    }
    
    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else
            qualityDropdown.value = 3; // Обычно "High"
            
        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;
            
        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;
            
        // Устанавливаем выбранные значения в UI
        qualityDropdown.RefreshShownValue();
        resolutionDropdown.RefreshShownValue();
    }
}