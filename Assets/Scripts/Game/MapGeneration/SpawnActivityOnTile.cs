using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class SpawnActivityOnTile : MonoBehaviour
{


    public GameObject _questGiver;
    [SerializeField] private Transform _questGiverSpawnPoint;

    public GameObject _mutationChest;
    [SerializeField] private Transform _mutationChestSpawnPoint;

    public GameObject _gasTank;
    [SerializeField] private Transform _gasTankSpawnPoint;

    [NonSerialized]public List<ActivityOnTileType> _objectsOnTile = new List<ActivityOnTileType>();


    public void SpawnGasTank()
    {
        GameObject GasTankSpawned = Instantiate(_gasTank,
           new Vector3(_gasTankSpawnPoint.position.x, _gasTank.transform.position.y, _gasTankSpawnPoint.position.z),
           Quaternion.Euler(0, Random.Range(0,360f), 0),
           transform);
    }


    public void SpawnQuestGiverOfType( QuestData Quest, ItemType QuestType)
    {
        GameObject QuestGiverSpawned = Instantiate(_questGiver,
            new Vector3(_questGiverSpawnPoint.position.x, _questGiver.transform.position.y, _questGiverSpawnPoint.position.z), 
            Quaternion.Euler(0, 180, 0), 
            transform);
        QuestGiverSpawned.GetComponent<QuestGiver>().SetQuestType(QuestType);
        QuestGiverSpawned.GetComponent<QuestGiver>().SetQuest(Quest);
    }
    public void SpawnMutationChest()
    {
        GameObject MutationSpawned = Instantiate(_mutationChest,
            new Vector3(_mutationChestSpawnPoint.position.x, _mutationChest.transform.position.y, _mutationChestSpawnPoint.position.z),
            Quaternion.Euler(0, 180, 0),
            transform);
    }

    public void SpawnTransitionQuest(QuestData Quest)
    {
        GameObject QuestGiverSpawned = Instantiate(_questGiver,
            new Vector3(_questGiverSpawnPoint.position.x, _questGiver.transform.position.y, _questGiverSpawnPoint.position.z),
            Quaternion.Euler(0,180,0),
            transform);
        QuestGiverSpawned.GetComponent<QuestGiver>()._transitionQuest = true;
        QuestGiverSpawned.GetComponent<QuestGiver>().SetQuest(Quest);
    }

}
