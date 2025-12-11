using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    public AudioClip newMusic;

    void Awake()
    {
        AudioManager.Instance.PlayMusic(newMusic);
    }

}
