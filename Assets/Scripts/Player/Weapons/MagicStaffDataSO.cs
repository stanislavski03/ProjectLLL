using UnityEngine;

[CreateAssetMenu(fileName = "New Magic Staff Data", menuName = "Weapons/Magic Staff Data")]
public class MagicStaffDataSO : WeaponDataSO
{
    [Header("Magic Staff Specific")]
    public float explosionArea = 1f;
    public float explosionLifetime = 2f;
    public float explosionCooldown = 1f;
    public GameObject explosionPrefab;
}