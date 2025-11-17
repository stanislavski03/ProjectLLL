using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement playerMovement;
    private float currentSpeed;
    public GameObject DamageGiveShield;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        currentSpeed = playerMovement.NormalizedSpeed;
    }

    void Update()
    {
        currentSpeed = playerMovement.NormalizedSpeed;

        //Debug.Log("playerMovement.CurrentSpeed = " + playerMovement.NormalizedSpeed);

        animator.SetFloat("PlayerSpeed", currentSpeed);

        if (currentSpeed > 0.1f)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }
}