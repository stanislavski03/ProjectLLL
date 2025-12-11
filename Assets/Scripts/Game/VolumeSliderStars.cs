using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderStars : MonoBehaviour
{
    public Slider MasterSlider;
    public Slider VFXSlider;
    public Slider MusicSlider;

    void OnEnable()
    {
        MasterSlider.value = AudioManager.Instance.GetMasterVolume();
        VFXSlider.value = AudioManager.Instance.GetSFXVolume();
        MusicSlider.value = AudioManager.Instance.GetMusicVolume();
    }
}
