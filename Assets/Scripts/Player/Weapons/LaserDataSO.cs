using UnityEngine;

[CreateAssetMenu(fileName = "New Laser Data", menuName = "Weapons/Laser Data")]
public class LaserDataSO : WeaponDataSO
{
    [Header("Laser Specific")]
    public float laserLength = 5f;
    public float laserLifetime = 2f;
    public float laserArea = 1f;
    public Material laserMaterial;
}