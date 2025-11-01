using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private PlayerInput _playerInput;
    private Vector2 _moveDirection;
    private Rigidbody rb;
    private Vector3 _lastVelocity;

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
        if (isPaused) return;

        float scaledMoveSpeed = _moveSpeed;
        Vector3 offset = new Vector3(_moveDirection.x, 0f, _moveDirection.y) * scaledMoveSpeed;
        rb.velocity = offset;
    }

}