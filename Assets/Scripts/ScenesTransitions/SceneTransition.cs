using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            TransitPlayerToNextLevel(1);
            Generation.Instance.GenerateMap(6,6);
        }
    }
    private void TransitPlayerToNextLevel(int number)
    {
        SceneManager.LoadScene(number);
    }
}
