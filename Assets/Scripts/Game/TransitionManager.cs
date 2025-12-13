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
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsPaused)
            GameStateManager.Instance.ResumeGame();



        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            EnemySpawnManager.Instance.enabled = true;
        }

        if (i == 0)
        {
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

        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(i);
        if (QuestManager.Instance != null && QuestManager.Instance.activeQuests.Count > 0)
        {
            foreach (QuestData quest in QuestManager.Instance.activeQuests)
            {
                QuestManager.Instance.CancelQuest(quest);
            }
        }
        if (QuestManager.Instance != null && QuestManager.Instance.completedQuests.Count > 0)
        {
            foreach (QuestData quest in QuestManager.Instance.completedQuests)
            {
                QuestManager.Instance.CancelQuest(quest);
            }
        }
        if (i == 0)
        {
            // Cursor.visible = true;
            // Cursor.lockState = CursorLockMode.None;
        }
    }

}
