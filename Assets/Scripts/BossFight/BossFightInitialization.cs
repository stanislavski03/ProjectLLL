using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightInitialization : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       Player.Instance.transform.position = new Vector3(50,0,10); 
    }


}
