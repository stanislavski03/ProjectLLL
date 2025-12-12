using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClickSound : MonoBehaviour
{

    public AudioClip uiClickClip;
    public void UiClickSound()
    {
        AudioManager.Instance.PlayClick(uiClickClip);
    }
}
