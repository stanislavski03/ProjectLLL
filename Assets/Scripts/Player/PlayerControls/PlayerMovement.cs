using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO _statsSO;

    private PlayerInput _playerInput;
    private Vector2 _moveDirection;
    private Rigidbody rb;
    
    // Для плавного изменения скорости
    private float _currentVelocity;
    private float _targetSpeed;
    
    // Параметры плавности
    [SerializeField] private float _acceleration = 45f;
    [SerializeField] private float _deceleration = 120f;
    
    // Свойство для доступа к скорости извне (для аниматора)
    public float CurrentSpeed => _currentVelocity;
    public float NormalizedSpeed => _currentVelocity / _statsSO.MoveSpeed;

    private bool isPaused;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isPaused) return;
        _moveDirection = _playerInput.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (isPaused) return;
        
        Move();
        CalculateSpeed();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Move()
    {
        // Определяем целевую скорость
        _targetSpeed = _moveDirection.magnitude > 0.1f ? _statsSO.MoveSpeed : 0f;
        
        // Плавно изменяем текущую скорость
        float accelerationRate = _moveDirection.magnitude > 0.1f ? _acceleration : _deceleration;
        _currentVelocity = Mathf.MoveTowards(_currentVelocity, _targetSpeed, accelerationRate * Time.fixedDeltaTime);
        
        // Применяем движение
        Vector3 moveInput = new Vector3(_moveDirection.x, 0f, _moveDirection.y);
        Vector3 moveDirection = moveInput.normalized * _currentVelocity;
        
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        
        // Поворот
        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 10 * Time.fixedDeltaTime);
        }
    }

    private void CalculateSpeed()
    {
        //если нужны дополнительные расчеты
    }
}