using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        // Смотрим в противоположную сторону от камеры
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
    }
}