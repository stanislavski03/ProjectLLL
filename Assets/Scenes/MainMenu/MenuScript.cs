using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void Play(int levelIndex)
    {
        TransitionManager.Instance.TransitPlayerToLevel(levelIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
