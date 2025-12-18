using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

public class Generation : MonoBehaviour
{
    [SerializeField] private GameObject _edgeWall;
    [SerializeField] private GameObject _edgeWallCorner;
    [SerializeField] private List<GameObject> _tilesVariationList = new List<GameObject>();
    [SerializeField] private int _generationHeight = 3;
    [SerializeField] private int _generationWidth = 3;
    [SerializeField] private List<QuestData> transitionQuests = new List<QuestData>();

    public static Generation Instance { get; private set; }

    private List<List<GameObject>> generation = new List<List<GameObject>>();
    private NavMeshSurface[] allSurfaces;
    public List<QuestData> _availableQuests = new List<QuestData>();
    public List<QuestData> _allQuests = new List<QuestData>();

    public int _questsOnMapMin = 2;
    public int _questsOnMapMax = 3;

    public int _mutationsOnMapMin = 3;
    public int _mutationsOnMapMax = 4;


    [SerializeField] private ItemControllerSO _itemController;



    
    public void SpawnGasTanks(int amount)
    {

        List<GameObject> allTiles = new List<GameObject>();
        foreach (var row in generation)
        {
            foreach (var tile in row)
            {
                if (tile != null)
                    allTiles.Add(tile);
            }
        }
        for (int i = 0; i < amount; i++)
        {
            int rand = Random.Range(0, allTiles.Count);
            allTiles[rand].GetComponent<SpawnActivityOnTile>().SpawnGasTank();
            allTiles.Remove(allTiles[rand]);

        }


    }


    public bool CheckPointForLegitment(Vector3 point)
    {
        if (point.z < 0 || point.x < 0 || point.z > (_generationHeight * 50) || point.x > (_generationWidth * 50))
        {
            return false;
        }
        else
            return true;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


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
        try
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
                SpawnActivityOnTile tileSpawnActivity = allTiles[i].GetComponent<SpawnActivityOnTile>();
                if (tileSpawnActivity != null)
                {
                    tileSpawnActivity._objectsOnTile.Add(ActivityOnTileType.Mutation);
                    tileSpawnActivity.SpawnMutationChest();
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"AddMutationsChestsToTiles error: {e.Message}");
        }
    }

    public void AddQuestsToTiles(int _width, int _height, int QuestsMin, int QuestsMax)
    {
        try
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

            ////// Transition Quests //////
            int transitionQuestSide = Random.Range(0, 4);
            QuestData TransitionQuest = transitionQuests[Random.Range(0, transitionQuests.Count)];
            switch (transitionQuestSide)
            {
                case 0:
                    SpawnActivityOnTile transitionTile = generation[0][Random.Range(0, _height)].GetComponent<SpawnActivityOnTile>();
                    transitionTile.SpawnTransitionQuest(TransitionQuest);
                    transitionTile._objectsOnTile.Add(ActivityOnTileType.Quest);
                    allTiles.Remove(transitionTile.gameObject);
                    break;
                case 1:
                    SpawnActivityOnTile transitionTile2 = generation[_width - 1][Random.Range(0, _height)].GetComponent<SpawnActivityOnTile>();
                    transitionTile2.SpawnTransitionQuest(TransitionQuest);
                    transitionTile2._objectsOnTile.Add(ActivityOnTileType.Quest);
                    allTiles.Remove(transitionTile2.gameObject);
                    break;
                case 2:
                    SpawnActivityOnTile transitionTile3 = generation[Random.Range(0, _width)][0].GetComponent<SpawnActivityOnTile>();
                    transitionTile3.SpawnTransitionQuest(TransitionQuest);
                    transitionTile3._objectsOnTile.Add(ActivityOnTileType.Quest);
                    allTiles.Remove(transitionTile3.gameObject);

                    break;
                case 3:
                    SpawnActivityOnTile transitionTile4 = generation[Random.Range(0, _width)][_height - 1].GetComponent<SpawnActivityOnTile>();
                    transitionTile4.SpawnTransitionQuest(TransitionQuest);
                    transitionTile4._objectsOnTile.Add(ActivityOnTileType.Quest);
                    allTiles.Remove(transitionTile4.gameObject);
                    break;
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
                SpawnActivityOnTile tileSpawnActivity = tile.GetComponent<SpawnActivityOnTile>();

                if (tileSpawnActivity != null && !tileSpawnActivity._objectsOnTile.Exists(activity => activity == ActivityOnTileType.Quest))
                {
                    ItemType QuestRewardType;
                    List<ItemType> questTypeList = new List<ItemType>();
                    if (_itemController.CheckPool(ItemType.Tecno))
                    {
                        questTypeList.Add(ItemType.Tecno);
                    }
                    if (_itemController.CheckPool(ItemType.Magic))
                    {
                        questTypeList.Add(ItemType.Magic);
                    }
                    if (_itemController.CheckPool(ItemType.Universal))
                    {
                        questTypeList.Add(ItemType.Universal);
                    }

                    QuestRewardType = questTypeList[Random.Range(0,questTypeList.Count)];

                    QuestData questData = _availableQuests[Random.Range(0, _availableQuests.Count)];
                    _availableQuests.Remove(questData);

                    tileSpawnActivity._objectsOnTile.Add(ActivityOnTileType.Quest);
                    tileSpawnActivity.SpawnQuestGiverOfType(questData, QuestRewardType);
                }
                
            }
        }
        catch
        {
            
        }
    }

    private void GenerateEdjeWalls()
    {
        if (generation.Count == 0) return;

        // Получаем границы карты
        float minX = generation[0][0].transform.position.x;
        float maxX = generation[generation.Count - 1][generation[0].Count - 1].transform.position.x; 
        float minZ = generation[0][0].transform.position.z;
        float maxZ = generation[0][generation[0].Count - 1].transform.position.z;

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

        // Добавляем угловые стены
        if (_edgeWallCorner != null)
        {
            GameObject northWestCorner = Instantiate(
                _edgeWallCorner,
                new Vector3(minX - 50, 0, minZ - 50),
                Quaternion.Euler(0, -90, 0),
                transform
            );

            GameObject northEastCorner = Instantiate(
                _edgeWallCorner,
                new Vector3(maxX + 50, 0, minZ - 50),
                Quaternion.Euler(0, -180, 0),
                transform
            );

            GameObject southWestCorner = Instantiate(
                _edgeWallCorner,
                new Vector3(minX - 50, 0, maxZ + 50),
                Quaternion.identity,
                transform
            );

            GameObject southEastCorner = Instantiate(
                _edgeWallCorner,
                new Vector3(maxX + 50, 0, maxZ + 50),
                Quaternion.Euler(0, 90, 0),
                transform
            );
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
                        Quaternion.Euler(0, Random.Range(0,4) * 90,0),
                        transform
                    );
                    row.Add(NewTile);
                }
            }

            AddMutationsChestsToTiles(_width, _height, _mutationsOnMapMin, _mutationsOnMapMax);
            AddQuestsToTiles(_width, _height, _questsOnMapMin, _questsOnMapMax);
            
            SetupNavMeshSurface();
            GenerateEdjeWalls();
            Vector3 spawnpoint = generation[_width / 2][_height / 2].transform.position;
            Player.Instance.transform.position = spawnpoint;
        }
    }

    void SetupNavMeshSurface()
    {
        if (allSurfaces != null)
        {
            foreach (NavMeshSurface surface in allSurfaces)
            {
                surface.collectObjects = CollectObjects.Children;
                surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
                BuildNavMesh(surface);
            }
        }
    }

    public void BuildNavMesh(NavMeshSurface parentSurface)
    {
        parentSurface.BuildNavMesh();
    }

    public void ClearGeneration()
    {
        // ������� NavMesh
        if (allSurfaces != null)
        {
            foreach (NavMeshSurface surface in allSurfaces)
            {
                surface.RemoveData();
                surface.navMeshData = null;
            }
            NavMesh.RemoveAllNavMeshData();
        }

        // ������� �������� ��������
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        generation.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GenerateMap(_generationHeight, _generationWidth);
        }
    }

    private void Start()
    {
        allSurfaces = GetComponents<NavMeshSurface>();
        GenerateMap(_generationHeight, _generationWidth);
    }
}