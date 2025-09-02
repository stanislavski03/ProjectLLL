using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private PlayerInput _playerInput;

    private Vector2 _moveDirection;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Player.Click.performed += OnClick;
    }

    private void Update()
    {
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
        if (_moveDirection.sqrMagnitude < 0.1f)
            return;

        float scaledMoveSpeed = _moveSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(_moveDirection.x, 0f, _moveDirection.y) * scaledMoveSpeed;

        transform.Translate(offset, Space.World);
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (context.interaction is MultiTapInteraction || context.interaction is HoldInteraction)
        {
            StartHeavyAttack();
        }
    }

    private void StartHeavyAttack()
    {
        Debug.Log("Heavy Attack");
    }
}