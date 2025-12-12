using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static TransitionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TransitPlayerToNextLevel()
    {
        TransitPlayerToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void TransitPlayerToLevel(int i)
    {
        if (GameStateManager.Instance.IsPaused)
            GameStateManager.Instance.ResumeGame();

        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            EnemySpawnManager.Instance.enabled = true;
        }

        if (i == 0) {
            GameObject[] bullshit = FindObjectsOfType<GameObject>();

            foreach (GameObject go in bullshit)
            {
                if (go.GetComponent<DontDestroyOnLoadScript>())
                {
                    Destroy(go);
                }
            }
        }
        else if (i == 3)
        { 
            EnemySpawnManager.Instance.enabled = false;
        }


            SceneManager.LoadScene(i);
    }

}
