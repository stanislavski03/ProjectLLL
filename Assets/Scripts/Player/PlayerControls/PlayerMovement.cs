using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO _statsSO;

    private PlayerInput _playerInput;
    private Vector2 _moveInput;
    private Rigidbody rb;
    
    public bool IsMoving { get; private set; }

    private bool isPaused;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isPaused) return;
        _moveInput = _playerInput.Player.Move.ReadValue<Vector2>();

        IsMoving = _moveInput.magnitude > 0.1f;
    }

    private void FixedUpdate()
    {
        if (isPaused) return;
        
        Move();
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
        Vector3 moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);
        
        if (moveDirection.magnitude > 0.1f)
        {
            moveDirection = moveDirection.normalized * _statsSO.MoveSpeed;
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
            
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 10 * Time.fixedDeltaTime);
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }
}