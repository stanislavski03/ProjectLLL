using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestPicker : MonoBehaviour
{
    public QuestData quest;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            quest.OnQuestStart();
            Destroy(gameObject);
        }
    }
}
