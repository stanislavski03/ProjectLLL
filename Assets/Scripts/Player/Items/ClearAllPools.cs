using UnityEngine;

public class ClearAllPools : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ItemControllerSO.Instance.ClearAllPools();
            WeaponController.Instance.ClearPool();
            MutationControllerSO.Instance.ClearAllMutations();
        }
    }
}