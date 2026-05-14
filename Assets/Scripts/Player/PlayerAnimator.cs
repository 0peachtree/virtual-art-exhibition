using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Animator Parameter Names")]
    [SerializeField] private string speedParameter = "Speed";
    [SerializeField] private string groundedParameter = "IsGrounded";
    [SerializeField] private string jumpingParameter = "IsJumping";

    [Header("Smoothing")]
    [SerializeField] private float speedSmoothTime = 0.1f;

    private PlayerController playerController;

    private float smoothedSpeed;
    private float speedVelocity;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        if (animator == null || playerController == null)
            return;

        UpdateAnimatorParameters();
    }

    private void UpdateAnimatorParameters()
    {
        smoothedSpeed = Mathf.SmoothDamp(
            smoothedSpeed,
            playerController.NormalizedMoveSpeed,
            ref speedVelocity,
            speedSmoothTime
        );

        bool isGrounded = playerController.IsGrounded;

        bool isJumping = !isGrounded;

        animator.SetFloat(speedParameter, smoothedSpeed);
        animator.SetBool(groundedParameter, isGrounded);
        animator.SetBool(jumpingParameter, isJumping);
    }
}