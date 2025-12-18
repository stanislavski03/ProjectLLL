using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magic Blizzard Quest", menuName = "Quests/Magic Blizzard Quest")]
public class MagicBlizzardQuest : QuestData
{
    [Header("Magic Blizzard Quest Settings")]
    public GameObject BlizzardShield;
    public float damagePeriod;
    public int damage;

    private GameObject ShieldRef;
    public override void OnQuestStart()
    {
        base.OnQuestStart();
        ShieldRef = Instantiate(BlizzardShield, _questGiver.transform.position, Quaternion.identity); 
        ShieldRef.GetComponent<BlizzardFieldTrigger>().damage = damage;
        ShieldRef.GetComponent<BlizzardFieldTrigger>().damagePeriod = damagePeriod;
        QuestManager.Instance?.RegisterQuest(this);
        EnemySpawnManager.Instance.GainPoints(25);

    }

    public override void OnQuestFinish()
    {
        base.OnQuestFinish();
        Destroy(ShieldRef);
        QuestManager.Instance?.UnregisterQuest(this);

            EnemySpawnManager.Instance.GainDifficulty(0.2f);



    }
    
    public override void OnQuestCancel()
    {
        base.OnQuestCancel();
        Destroy(ShieldRef);
        QuestManager.Instance?.CancelQuest(this);
        
    }
}