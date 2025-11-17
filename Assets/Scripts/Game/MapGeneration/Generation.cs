using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Generation : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjectsList = new List<GameObject>();
    List<List<GameObject>> generation = new List<List<GameObject>>();
    NavMeshSurface[] allSurfaces;
    public void GenerateMap(int _width, int _height)
    {
        if (_width != 0 && _height != 0)
        {
            ClearGeneration();
            for (int i = 0; i < _width; i++)
            {
                List<GameObject> row = new List<GameObject>();
                generation.Add(row);

                for (int j = 0; j < _height; j++)
                {

                    GameObject newObject = Instantiate(
                        gameObjectsList[Random.Range(0, gameObjectsList.Count)],
                        new Vector3(transform.position.x + i * 50, 0, transform.position.z + j * 50),
                        Quaternion.identity,
                        transform
                    );

                    row.Add(newObject);
                }
            }
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
            GenerateMap(3, 2);
        }
    }
    private void Start()
    {
        allSurfaces = GetComponents<NavMeshSurface>();
        GenerateMap(3, 2);
    }
}