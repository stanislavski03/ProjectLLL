using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement playerMovement;
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
    }

    void Update()
    {
        animator.SetBool("IsMoving", playerMovement.IsMoving);
    }
}