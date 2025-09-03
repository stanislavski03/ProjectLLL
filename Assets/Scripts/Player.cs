using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
//     [SerializeField] private float _moveSpeed;
//     [SerializeField] private Transform _bulletPrefab;
//     [SerializeField] private Transform _bulletSpawn;
//     [SerializeField] private float _shootSpeed;

//     private PlayerInput _playerInput;

//     private Vector2 _moveDirection;

//     private EnemyDetector _enemyDetector;

//     private void Awake()
//     {
//         _playerInput = new PlayerInput();
//         _enemyDetector = GetComponent<EnemyDetector>();

//         _playerInput.Player.Click.performed += OnClick;
//     }

//     private void Start()
//     {
//         InvokeRepeating(nameof(ShootClosestEnemy), 0f, 0.5f);
//     }

//     private void Update()
//     {
//         _moveDirection = _playerInput.Player.Move.ReadValue<Vector2>();

//         Move();
//     }

//     private void OnEnable()
//     {
//         _playerInput.Enable();
//     }

//     private void OnDisable()
//     {
//         _playerInput.Disable();
//     }

//     private void Move()
//     {
//         if (_moveDirection.sqrMagnitude < 0.1f)
//             return;

//         float scaledMoveSpeed = _moveSpeed * Time.deltaTime;
//         Vector3 offset = new Vector3(_moveDirection.x, 0f, _moveDirection.y) * scaledMoveSpeed;

//         transform.Translate(offset, Space.World);
//     }

//     private void OnClick(InputAction.CallbackContext context)
//     {
//         if (context.interaction is MultiTapInteraction || context.interaction is HoldInteraction)
//         {
//             StartHeavyAttack();
//         }
//     }

//     private void ShootClosestEnemy()
//     {
//         Enemy closestEnemy = _enemyDetector.GetClosestEnemy();
//         if (closestEnemy != null)
//         {
//             Shoot(closestEnemy);
//         }
//     }

//     private void StartHeavyAttack()
//     {
//         Debug.Log("Heavy Attack");
//     }

//     private void Shoot(Enemy enemy)
// {
//     GameObject bulletObj = BulletPool.Instance.GetBullet();
//     bulletObj.transform.position = _bulletSpawn.position;
//     bulletObj.transform.rotation = _bulletSpawn.rotation;
    
//     Bullet bulletController = bulletObj.GetComponent<Bullet>();
//     if (bulletController != null)
//     {
//         bulletController.ResetBullet(enemy.transform, _shootSpeed);
//     }
// }
}