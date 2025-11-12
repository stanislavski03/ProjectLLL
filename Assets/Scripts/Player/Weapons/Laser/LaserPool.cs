using System.Collections.Generic;
using UnityEngine;

public class LaserPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private int initialPoolSize = 5;
    [SerializeField] private int expandAmount = 3;

    private Queue<LaserBeam> laserPool = new Queue<LaserBeam>();
    private List<LaserBeam> allLasers = new List<LaserBeam>();

    public static LaserPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        ExpandPool(initialPoolSize);
        Debug.Log($"LaserPool initialized with {initialPoolSize} lasers");
    }

    private void ExpandPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateNewLaser();
        }
    }

    private void CreateNewLaser()
    {
        // СОЗДАЕМ ЧЕРЕЗ Instantiate С ПАРАМЕТРОМ parent
        GameObject laserObject = new GameObject("PooledLaser", typeof(LaserBeam));

        // УСТАНАВЛИВАЕМ РОДИТЕЛЯ ПРИ СОЗДАНИИ
        laserObject.transform.SetParent(transform, false);
        laserObject.transform.localPosition = Vector3.zero;
        laserObject.transform.localRotation = Quaternion.identity;

        LaserBeam laserBeam = laserObject.GetComponent<LaserBeam>();

        laserObject.SetActive(false);
        laserPool.Enqueue(laserBeam);
        allLasers.Add(laserBeam);
    }

    public LaserBeam GetLaser(float length, float lifetime, float damage, Material material, float area, Laser source, Vector3 startPos, Vector3 targetPos)
    {
        CleanPool();

        LaserBeam laserBeam = null;

        // Ищем в пуле
        while (laserPool.Count > 0 && laserBeam == null)
        {
            laserBeam = laserPool.Dequeue();
            if (laserBeam == null) continue;
        }

        // Если пул пуст - создаем новый
        if (laserBeam == null)
        {
            Debug.Log("Pool empty, creating new laser");
            CreateNewLaser();
            if (laserPool.Count > 0)
                laserBeam = laserPool.Dequeue();
        }

        if (laserBeam != null)
        {
            // Активируем и инициализируем
            laserBeam.gameObject.SetActive(true);
            laserBeam.Initialize(length, lifetime, damage, material, area, source, startPos, targetPos);
        }

        return laserBeam;
    }

    public void ReturnLaser(LaserBeam laserBeam)
    {
        if (laserBeam == null) return;

        // Деактивируем и возвращаем в пул
        laserBeam.gameObject.SetActive(false);
        laserBeam.transform.SetParent(transform);
        laserBeam.transform.position = Vector3.zero;
        laserBeam.transform.rotation = Quaternion.identity;

        if (!laserPool.Contains(laserBeam))
        {
            laserPool.Enqueue(laserBeam);
        }
    }

    private void CleanPool()
    {
        Queue<LaserBeam> cleanPool = new Queue<LaserBeam>();

        while (laserPool.Count > 0)
        {
            LaserBeam laser = laserPool.Dequeue();
            if (laser != null)
            {
                cleanPool.Enqueue(laser);
            }
        }

        laserPool = cleanPool;
        allLasers.RemoveAll(laser => laser == null);
    }

    // Для дебага
    private void Update()
    {
        if (Time.frameCount % 300 == 0) // Каждые 5 секунд
        {
            Debug.Log($"Laser Pool: {laserPool.Count} available, {allLasers.Count} total");
        }
    }
}