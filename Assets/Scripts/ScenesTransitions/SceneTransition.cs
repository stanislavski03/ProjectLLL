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
            TransitionManager.Instance.TransitPlayerToNextLevel();
            Generation.Instance.GenerateMap(6,6);
        }
    }
}
