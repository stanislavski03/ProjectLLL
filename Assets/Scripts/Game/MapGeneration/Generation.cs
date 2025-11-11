using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjectsList = new List<GameObject>();
    List<List<GameObject>> generation = new List<List<GameObject>>();

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
                        Quaternion.Euler(Vector3.up * Random.Range(0, 3) * 90f),
                        transform
                    );

                    row.Add(newObject);
                }
            }
        }
    }

    public void ClearGeneration()
    {

            generation.Clear();
        foreach (Transform child in transform)
        {

            Destroy(child.gameObject);

        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GenerateMap(5, 5);
        }
    }
    private void Start()
    {
        GenerateMap(5, 5);
    }
}