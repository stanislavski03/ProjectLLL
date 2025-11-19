using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using UnityEngine.SocialPlatforms;

public class Generation : MonoBehaviour
{
    [SerializeField] private GameObject _edgeWall;
    [SerializeField] private List<GameObject> _tilesVariationList = new List<GameObject>();
    private List<List<GameObject>> generation = new List<List<GameObject>>();
    private NavMeshSurface[] allSurfaces;
    public List<QuestData> _availableQuests = new List<QuestData>();
    public List<QuestData> _allQuests = new List<QuestData>();

    public int _questsOnMapMin = 2;
    public int _questsOnMapMax = 3;

    public int _mutationsOnMapMin = 3;
    public int _mutationsOnMapMax = 4;

    public void InitialiseGenerationList(int _width, int _height)
    {
        ClearGeneration();
        for (int i = 0; i < _width; i++)
        {
            List<GameObject> row = new List<GameObject>();
            generation.Add(row);

            for (int j = 0; j < _height; j++)
            {
                row.Add(null);
            }
        }
    }

    public void AddMutationsChestsToTiles(int _width, int _height, int MutationsMin, int MutationsMax)
    {
        
        if (MutationsMin < 0)
            MutationsMin = 0;


        int maxPossibleMutations = _width * _height;
        if (MutationsMax > maxPossibleMutations)
            MutationsMax = maxPossibleMutations;

        if (MutationsMin > MutationsMax)
            MutationsMin = MutationsMax;

        int MutationsAmount;
        if (MutationsMax == MutationsMin)
            MutationsAmount = MutationsMax;
        else
            MutationsAmount = Random.Range(MutationsMin, MutationsMax + 1);


        List<GameObject> allTiles = new List<GameObject>();
        foreach (var row in generation)
        {
            foreach (var tile in row)
            {
                if (tile != null)
                    allTiles.Add(tile);
            }
        }


        if (MutationsAmount > allTiles.Count)
            MutationsAmount = allTiles.Count;


        for (int i = 0; i < allTiles.Count; i++)
        {
            int randomIndex = Random.Range(i, allTiles.Count);
            GameObject temp = allTiles[i];
            allTiles[i] = allTiles[randomIndex];
            allTiles[randomIndex] = temp;
        }


        for (int i = 0; i < MutationsAmount && i < allTiles.Count; i++)
        {
            SpawnActivity tileSpawnActivity = allTiles[i].GetComponent<SpawnActivity>();
            if (tileSpawnActivity != null)
            {
                tileSpawnActivity._objectsOnTile.Add(ActivityOnTileType.Mutation);
                tileSpawnActivity.SpawnMutationChest();
            }
        }
        
        
    }

    public void AddQuestsToTiles(int _width, int _height, int QuestsMin, int QuestsMax)
    {
        
        if (QuestsMin < 0)
            QuestsMin = 0;


        int maxPossibleQuests = _width * _height;
        if (QuestsMax > maxPossibleQuests)
            QuestsMax = maxPossibleQuests;

        if (QuestsMin > QuestsMax)
            QuestsMin = QuestsMax;

        int QuestAmount = Random.Range(QuestsMin, QuestsMax + 1);


        List<GameObject> allTiles = new List<GameObject>();
        foreach (var row in generation)
        {
            foreach (var tile in row)
            {
                if (tile != null)
                    allTiles.Add(tile);
            }
        }


        if (_availableQuests.Count <= 0 || allTiles.Count <= 0)
        {
            return;
        }


        QuestAmount = Mathf.Min(QuestAmount, allTiles.Count, _availableQuests.Count);


        for (int i = 0; i < allTiles.Count; i++)
        {
            int randomIndex = Random.Range(i, allTiles.Count);
            GameObject temp = allTiles[i];
            allTiles[i] = allTiles[randomIndex];
            allTiles[randomIndex] = temp;
        }

        for (int i = 0; i < QuestAmount; i++)
        {
            GameObject tile = allTiles[i];
            SpawnActivity tileSpawnActivity = tile.GetComponent<SpawnActivity>();

            if (tileSpawnActivity != null && !tileSpawnActivity._objectsOnTile.Exists(activity => activity == ActivityOnTileType.Quest))
            {
                ItemType QuestRewardType;
                switch (Random.Range(0, 3))
                {
                    case 0:
                        QuestRewardType = ItemType.Tecno;
                        break;
                    case 1:
                        QuestRewardType = ItemType.Magic;
                        break;
                    default:
                        QuestRewardType = ItemType.Universal;
                        break;
                }

                QuestData questData = _availableQuests[Random.Range(0, _availableQuests.Count)];
                _availableQuests.Remove(questData);

                tileSpawnActivity._objectsOnTile.Add(ActivityOnTileType.Quest);
                tileSpawnActivity.SpawnQuestGiverOfType(questData, QuestRewardType);
            }
        }
       
       
    }

    private void GenerateEdjeWalls()
    {
        if (generation.Count == 0) return;

        foreach (List<GameObject> tileList in generation)
        {
            if (tileList.Count == 0) continue;

            GameObject NewNorthWall = Instantiate(
                _edgeWall,
                new Vector3(tileList[0].transform.position.x, 0, tileList[0].transform.position.z - 50),
                Quaternion.Euler(0, 180, 0),
                transform
            );
            GameObject NewSouthWall = Instantiate(
                _edgeWall,
                new Vector3(tileList[tileList.Count - 1].transform.position.x, 0, tileList[tileList.Count - 1].transform.position.z + 50),
                Quaternion.identity,
                transform
            );
        }

        if (generation[0].Count > 0)
        {
            foreach (GameObject tile in generation[0])
            {
                GameObject NewWestWall = Instantiate(
                    _edgeWall,
                    new Vector3(tile.transform.position.x - 50, 0, tile.transform.position.z),
                    Quaternion.Euler(0, -90, 0),
                    transform
                );
            }
        }

        if (generation.Count > 0 && generation[generation.Count - 1].Count > 0)
        {
            foreach (GameObject tile in generation[generation.Count - 1])
            {
                GameObject NewEastWall = Instantiate(
                    _edgeWall,
                    new Vector3(tile.transform.position.x + 50, 0, tile.transform.position.z),
                    Quaternion.Euler(0, 90, 0),
                    transform
                );
            }
        }
    }

    public void GenerateMap(int _width, int _height)
    {
        if (_width > 0 && _height > 0)
        {
            ClearGeneration();

            for (int i = 0; i < _width; i++)
            {
                List<GameObject> row = new List<GameObject>();
                generation.Add(row);

                for (int j = 0; j < _height; j++)
                {
                    GameObject NewTile = Instantiate(
                        _tilesVariationList[Random.Range(0, _tilesVariationList.Count)],
                        new Vector3(transform.position.x + i * 50, 0, transform.position.z + j * 50),
                        Quaternion.Euler(0,90 * Random.Range(0,4),0),
                        transform
                    );
                    row.Add(NewTile);
                }
            }

            AddMutationsChestsToTiles(_width, _height, _mutationsOnMapMin, _mutationsOnMapMax);
            AddQuestsToTiles(_width, _height, _questsOnMapMin, _questsOnMapMax);
            GenerateEdjeWalls();
            SetupNavMeshSurface();
        }
    }

    void SetupNavMeshSurface()
    {
        foreach (NavMeshSurface surface in allSurfaces)
        {
<<<<<<< Updated upstream

            surface.collectObjects = CollectObjects.Children;
            surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            BuildNavMesh(surface);
=======
            foreach (NavMeshSurface surface in allSurfaces)
            {
                surface.collectObjects = CollectObjects.Children;
                surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
                BuildNavMesh(surface);
            }
>>>>>>> Stashed changes
        }
    }

    public void BuildNavMesh( NavMeshSurface parentSurface)
    {       
        parentSurface.BuildNavMesh();
    }

    public void ClearGeneration()
    {
<<<<<<< Updated upstream

        generation.Clear();
        foreach (NavMeshSurface surface in allSurfaces)
        {
            surface.RemoveData();
            surface.navMeshData = null;
=======
        // Очистка NavMesh
        if (allSurfaces != null)
        {
            foreach (NavMeshSurface surface in allSurfaces)
            {
                surface.RemoveData();
                surface.navMeshData = null;
            }
            NavMesh.RemoveAllNavMeshData();
>>>>>>> Stashed changes
        }
        
        NavMesh.RemoveAllNavMeshData();
        foreach (Transform child in transform)
        {

<<<<<<< Updated upstream
            Destroy(child.gameObject);

=======
        // Очистка дочерних объектов
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
>>>>>>> Stashed changes
        }

        generation.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GenerateMap(3, 3);
        }
    }

    private void Start()
    {
        allSurfaces = GetComponents<NavMeshSurface>();
        GenerateMap(3, 3);
    }
}