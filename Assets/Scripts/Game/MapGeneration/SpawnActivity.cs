using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActivity : MonoBehaviour
{

    public GameObject _questGiver;
    [SerializeField] private Transform _questGiverSpawnPoint;

    public GameObject _mutationChest;
    [SerializeField] private Transform _mutationChestSpawnPoint;

    [NonSerialized]public List<ActivityOnTileType> _objectsOnTile = new List<ActivityOnTileType>();

    public void SpawnQuestGiverOfType( QuestData Quest, ItemType QuestType)
    {
        GameObject QuestGiverSpawned = Instantiate(_questGiver,
            new Vector3(_questGiverSpawnPoint.position.x, _questGiver.transform.position.y, _questGiverSpawnPoint.position.z), 
            Quaternion.identity, 
            transform);
        QuestGiverSpawned.GetComponent<QuestGiver>().SetQuestType(QuestType);
        QuestGiverSpawned.GetComponent<QuestGiver>().SetQuest(Quest);
    }
    public void SpawnMutationChest()
    {
        GameObject MutationSpawned = Instantiate(_mutationChest,
            new Vector3(_mutationChestSpawnPoint.position.x, _mutationChest.transform.position.y, _mutationChestSpawnPoint.position.z),
            Quaternion.identity,
            transform);
    }

    public void SpawnTransitionQuest(QuestData Quest)
    {
        GameObject QuestGiverSpawned = Instantiate(_questGiver,
            new Vector3(_questGiverSpawnPoint.position.x, _questGiver.transform.position.y, _questGiverSpawnPoint.position.z),
            Quaternion.identity,
            transform);
        QuestGiverSpawned.GetComponent<QuestGiver>()._transitionQuest = true;
        QuestGiverSpawned.GetComponent<QuestGiver>().SetQuest(Quest);
    }

}
