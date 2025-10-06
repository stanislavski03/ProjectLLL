using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet Shooter Data", menuName = "Weapons/Bullet Shooter Data")]
public class BulletShooterDataSO : WeaponDataSO
{
    [Header("Bullet Shooter Specific")]
    public float bulletSpeed = 10f;
    public float bulletLifetime = 3f;
    public int damageType = 0; // 0=freeze, 1=fire, 2=electro
    public GameObject bulletPrefab;
    
    [Header("Visual Effects")]
    public GameObject shootEffect;
    public AudioClip shootSound;
}