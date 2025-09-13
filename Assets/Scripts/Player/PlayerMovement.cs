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

    private void Awake()
    {
        _playerInput = new PlayerInput();
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        _playerInput.Player.Click.performed += OnClick;

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        CountdownController.OnCountdownStarted += OnCountdownStarted;
        CountdownController.OnCountdownFinished += OnCountdownFinished;
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

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        CountdownController.OnCountdownStarted -= OnCountdownStarted;
        CountdownController.OnCountdownFinished -= OnCountdownFinished;
    }

    private void Move()
    {
        

        float scaledMoveSpeed = _moveSpeed;
        Vector3 offset = new Vector3(_moveDirection.x, 0f, _moveDirection.y) * scaledMoveSpeed;

        rb.velocity = offset;
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

    private void OnCountdownStarted()
    {
        enabled = false;
    }

    private void OnCountdownFinished()
    {
        if (GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
        {
            enabled = true;
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        if (newGameState == GameState.Paused)
        {
            enabled = false;
        }
        else if (newGameState == GameState.Gameplay)
        {
            enabled = false;
        }
    }
}
