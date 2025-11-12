using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] private float baseWidthMultiplier = 0.005f;

    private float currentLength;
    private float currentLifetime;
    private float currentDamage;
    private float currentArea;
    private Material laserMaterial;
    private Laser laserSource;
    private Vector3 startPosition;
    private Vector3 targetDirection;

    private List<EnemyHP> damagedEnemies = new List<EnemyHP>();
    private bool isActive = false;
    private Coroutine lifecycleCoroutine;

    // Кэшируем компоненты
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private BoxCollider boxCollider;

    public bool IsActive => isActive;

    private void Awake()
    {
        // Кэшируем компоненты в Awake
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        // Если компонентов нет - создаем их
        if (meshFilter == null)
            meshFilter = gameObject.AddComponent<MeshFilter>();
        if (meshRenderer == null)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }
    }

    public void Initialize(float length, float lifetime, float damage, Material material, float area, Laser source, Vector3 startPos, Vector3 targetPos)
    {
        currentLength = length;
        currentLifetime = lifetime;
        currentDamage = damage;
        laserMaterial = material;
        currentArea = area;
        laserSource = source;
        startPosition = startPos;

        // Вычисляем направление
        Vector3 directionToTarget = (targetPos - startPos).normalized;
        targetDirection = directionToTarget;
        targetDirection.y = 1;

        SetupLaser(targetPos);

        if (gameObject.activeInHierarchy)
        {
            lifecycleCoroutine = StartCoroutine(LaserLifecycle());
        }
    }

    private void SetupLaser(Vector3 targetPosition)
    {
        // Вычисляем направление к врагу
        Vector3 directionToEnemy = (targetPosition - startPosition).normalized;

        // Создаем вращение только по оси Y (горизонтальное)
        Vector3 directionFlat = new Vector3(directionToEnemy.x, 0f, directionToEnemy.z).normalized;

        // Позиционируем луч: начало у игрока, луч тянется к врагу
        transform.position = startPosition; // Начало луча у игрока

        // Поворачиваем в направлении врага
        transform.rotation = Quaternion.LookRotation(directionFlat);

        // Создаем/обновляем меш
        CreateLaserMesh();

        // Настраиваем материал
        if (laserMaterial != null)
        {
            meshRenderer.material = laserMaterial;
        }
        else
        {
            meshRenderer.material = CreateDefaultMaterial();
            Debug.LogWarning("Laser material not assigned, using default");
        }

        // Вычисляем финальную ширину с учетом area множителя
        float finalWidth = currentLength * baseWidthMultiplier * currentArea;

        // Настраиваем коллайдер - смещаем центр вперед на половину длины
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(finalWidth, 0.1f, currentLength);
            boxCollider.center = new Vector3(0, 0, currentLength / 2f); // Центр смещен вперед
        }

        isActive = true;
        damagedEnemies.Clear();

        Debug.Log($"LaserBeam initialized: Start={startPosition}, Length={currentLength}, Width={finalWidth}");
    }

    private void CreateLaserMesh()
    {
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter is null in CreateLaserMesh!");
            return;
        }

        // Ширина = базовая ширина (25%) × множитель площади
        float width = currentLength * baseWidthMultiplier * currentArea;
        float halfWidth = width / 2f;

        // Вершины смещены вперед - луч начинается в нуле и тянется вперед
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
        new Vector3(-halfWidth, 0, 0),          // Левый ближний
        new Vector3(halfWidth, 0, 0),           // Правый ближний  
        new Vector3(-halfWidth, 0, currentLength), // Левый дальний
        new Vector3(halfWidth, 0, currentLength)   // Правый дальний
        };

        int[] triangles = new int[6]
        {
        0, 2, 1,
        2, 3, 1
        };

        Vector2[] uv = new Vector2[4]
        {
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(1, 1)
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    private Material CreateDefaultMaterial()
    {
        Material defaultMaterial = new Material(Shader.Find("Standard"));
        defaultMaterial.color = Color.red;
        defaultMaterial.SetFloat("_Metallic", 0f);
        defaultMaterial.SetFloat("_Glossiness", 0.9f);
        defaultMaterial.EnableKeyword("_EMISSION");
        defaultMaterial.SetColor("_EmissionColor", Color.red * 0.5f);
        return defaultMaterial;
    }

    private IEnumerator LaserLifecycle()
    {
        float timer = 0f;

        while (timer < currentLifetime && isActive)
        {
            DamageEnemiesInLaser();
            timer += 0.3f;
            yield return new WaitForSeconds(0.3f);
        }

        FinishLaser();
    }

    private void DamageEnemiesInLaser()
    {
        if (!isActive) return;

        // Вычисляем размеры с учетом area множителя
        float finalWidth = currentLength * baseWidthMultiplier * currentArea;

        // Центр коллайдера смещен вперед на половину длины
        Vector3 boxCenter = new Vector3(0, 0, currentLength / 2f);
        Vector3 halfExtents = new Vector3(
            finalWidth / 2f,
            0.5f,
            currentLength / 2f
        );

        // Преобразуем локальные координаты в мировые
        Vector3 worldCenter = transform.TransformPoint(boxCenter);

        Collider[] hitColliders = Physics.OverlapBox(
            worldCenter,
            halfExtents,
            transform.rotation
        );

        foreach (var hitCollider in hitColliders)
        {
            EnemyHP enemy = hitCollider.GetComponent<EnemyHP>();
            if (enemy != null && !damagedEnemies.Contains(enemy))
            {
                float actualDamage = laserSource != null ? laserSource.GetDamage() : currentDamage;
                enemy.Damage(actualDamage);
                damagedEnemies.Add(enemy);
                Debug.Log($"Laser damaged enemy: {actualDamage} damage");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        EnemyHP enemy = other.GetComponent<EnemyHP>();
        if (enemy != null && !damagedEnemies.Contains(enemy))
        {
            float actualDamage = laserSource != null ? laserSource.GetDamage() : currentDamage;
            enemy.Damage(actualDamage);
            damagedEnemies.Add(enemy);
        }
    }

    private void FinishLaser()
    {
        isActive = false;
        laserSource?.OnLaserFinished(this);
    }

    private void OnDisable()
    {
        if (lifecycleCoroutine != null)
        {
            StopCoroutine(lifecycleCoroutine);
            lifecycleCoroutine = null;
        }

        // Сбрасываем состояние
        isActive = false;
        damagedEnemies.Clear();
        laserSource = null;
        // НЕ сбрасываем материал - он будет переиспользован
    }

    // Визуализация в редакторе
    private void OnDrawGizmosSelected()
    {
        if (!isActive) return;

        Gizmos.color = Color.red;

        // Показываем реальное положение коллайдера
        float finalWidth = currentLength * baseWidthMultiplier * currentArea;
        Vector3 boxCenter = new Vector3(0, 0, currentLength / 2f);
        Vector3 worldCenter = transform.TransformPoint(boxCenter);
        Vector3 size = new Vector3(finalWidth, 0.1f, currentLength);

        Gizmos.matrix = Matrix4x4.TRS(worldCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);

        // Дополнительно рисуем линию от игрока
        Gizmos.color = Color.blue;
        Gizmos.matrix = Matrix4x4.identity;
        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * currentLength;
        Gizmos.DrawLine(start, end);
    }
}