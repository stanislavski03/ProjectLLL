using UnityEngine;

public class LightInitialisation : MonoBehaviour
{
    [SerializeField] private Vector3 DirectionNeeded;
    void Start()
    {
        Vector3 direction = DirectionNeeded - transform.position;
        transform.rotation = Quaternion.Euler(direction);
    }


}
