using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Field Data", menuName = "Weapons/Damage Field Data")]
public class DamageFieldDataSO : WeaponDataSO
{
    [Header("Damage Field Specific")]
    public float baseArea = 8f;
    
    [Header("Visual Effects")]
    public GameObject fieldEffectPrefab;
    public Color fieldColor = Color.red;
}