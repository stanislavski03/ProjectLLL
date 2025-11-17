using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement playerMovement;
    private float currentSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentSpeed = playerMovement.NormalizedSpeed;
    }

    void Update()
    {
        currentSpeed = playerMovement.NormalizedSpeed;

        Debug.Log("playerMovement.CurrentSpeed = " + playerMovement.NormalizedSpeed);

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