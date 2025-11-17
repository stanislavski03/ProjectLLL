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
        try
        {
            if (MutationsMin < 0)
                MutationsMin = 0;

            if (MutationsMax > _width * _height)
                MutationsMax = _width * _height;

            if (MutationsMin > MutationsMax)
                MutationsMin = MutationsMax;

            int MutationsAmount;
            if (MutationsMax == MutationsMin)
                MutationsAmount = MutationsMax;
            else
                MutationsAmount = Random.Range(MutationsMin, MutationsMax + 1);
            List<List<GameObject>> NotSpawned = new List<List<GameObject>>();

            foreach (var row in generation)
            {
                NotSpawned.Add(new List<GameObject>(row));
            }

            if (MutationsAmount >= _width * _height)
            {
                for (int i = 0; i < _width; i++)
                {
                    for (int j = 0; j < _height; j++)
                    {
                        NotSpawned[i][j].GetComponent<SpawnActivity>()._objectsOnTile.Add(ActivityOnTileType.Quest);
                        NotSpawned[i][j].GetComponent<SpawnActivity>().SpawnMutationChest();
                    }
                }
            }
            else
                for (int i = 0; i < MutationsAmount; i++)
                {
                    int _randWidth = Random.Range(0, NotSpawned.Count);
                    int _randHeight = Random.Range(0, NotSpawned[_randWidth].Count);

                    SpawnActivity TileToSpawnQuest = NotSpawned[_randWidth][_randHeight].GetComponent<SpawnActivity>();
                    TileToSpawnQuest._objectsOnTile.Add(ActivityOnTileType.Mutation);
                    TileToSpawnQuest.SpawnMutationChest();

                    NotSpawned[_randWidth].RemoveAt(_randHeight);
                    if (NotSpawned[_randWidth].Count == 0)
                        NotSpawned.RemoveAt(_randWidth);
                    if (NotSpawned.Count == 0)
                    {
                        return;
                    }
                }
        }
        catch { Debug.Log("AddMutationsChestsToTiles"); }
    }

    public void AddQuestsToTiles(int _width, int _height, int QuestsMin, int QuestsMax)
    {
        try
        {

            if (QuestsMin < 0)
                QuestsMin = 0;

            if (QuestsMax > _availableQuests.Count)
                QuestsMax = _availableQuests.Count;

            if (QuestsMax > _width * _height)
                QuestsMax = _width * _height;

            if (QuestsMin > QuestsMax)
                QuestsMin = QuestsMax;



            int QuestAmount = Random.Range(QuestsMin, QuestsMax + 1);

            List<List<GameObject>> NotSpawned = new List<List<GameObject>>();

            foreach (var row in generation)
            {
                NotSpawned.Add(new List<GameObject>(row));
            }

            for (int i = 0; i < QuestAmount; i++)
            {
                if (_availableQuests.Count <= 0)
                {
                    return;
                }
                int _randWidth = Random.Range(0, NotSpawned.Count);
                int _randHeight = Random.Range(0, NotSpawned[_randWidth].Count);


                GameObject TileToSpawnQuest = NotSpawned[_randWidth][_randHeight];
                SpawnActivity ScriptToSpawnQuest = TileToSpawnQuest.GetComponent<SpawnActivity>();


                if (ScriptToSpawnQuest._objectsOnTile.Exists(activity => activity == ActivityOnTileType.Quest))
                {
                    i--;
                }
                else
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
                    ScriptToSpawnQuest._objectsOnTile.Add(ActivityOnTileType.Quest);
                    ScriptToSpawnQuest.SpawnQuestGiverOfType(questData, QuestRewardType);

                    NotSpawned[_randWidth].RemoveAt(_randHeight);
                    if (NotSpawned[_randWidth].Count == 0)
                        NotSpawned.RemoveAt(_randWidth);



                }
            }
        }
        catch { Debug.Log("AddQuestsToTiles"); }
       

    }

    private void GenerateEdjeWalls()
    {
        foreach (List<GameObject> tileList in generation)
        {
            GameObject NewNorthWall = Instantiate(
                           _edgeWall,
                           new Vector3(tileList[0].transform.position.x, 0, tileList[0].transform.position.z - 50),
                           Quaternion.Euler(0, 180, 0),
                           transform
                       );
            GameObject NewSouthWall = Instantiate(
                           _edgeWall,
                           new Vector3(tileList[tileList.Count - 1].transform.position.x, 0, tileList[tileList.Count-1].transform.position.z +50),
                           Quaternion.identity,
                           transform
                       );
        }
        foreach (GameObject tile in generation[0])
        {
            GameObject NewNorthWall = Instantiate(
                           _edgeWall,
                           new Vector3(tile.transform.position.x-50, 0, tile.transform.position.z),
                           Quaternion.Euler(0, -90, 0),
                           transform
                       );
        }
        foreach (GameObject tile in generation[generation.Count -1])
        {
            GameObject NewNorthWall = Instantiate(
                           _edgeWall,
                           new Vector3(tile.transform.position.x +50, 0, tile.transform.position.z),
                           Quaternion.Euler(0,90,0),
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
                            Quaternion.identity,
                            transform
                        );
                        row.Add(NewTile);
                }
            }
            AddMutationsChestsToTiles(_width, _height, _mutationsOnMapMin, _mutationsOnMapMax);
            AddQuestsToTiles(_width,_height, _questsOnMapMin, _questsOnMapMax);
            GenerateEdjeWalls();
            SetupNavMeshSurface();


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
        if (allSurfaces != null)
        {
            generation.Clear();
            foreach (NavMeshSurface surface in allSurfaces)
            {
                surface.RemoveData();
                surface.navMeshData = null;
            }

            NavMesh.RemoveAllNavMeshData();
            foreach (Transform child in transform)
            {

                Destroy(child.gameObject);

            }
        }


    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GenerateMap(3, 4);
        }
    }
    private void Start()
    {
        allSurfaces = GetComponents<NavMeshSurface>(); 
        GenerateMap(3, 4);
    }
}