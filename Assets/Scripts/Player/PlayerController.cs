using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTarget;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 0.12f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    private CharacterController characterController;
    private PlayerInputHandler inputHandler;

    private float verticalVelocity;
    private float cameraPitch;
    private float currentMoveSpeed;
    private float normalizedMoveSpeed;

    public float CurrentMoveSpeed => currentMoveSpeed;
    public float NormalizedMoveSpeed => normalizedMoveSpeed;
    public float VerticalVelocity => verticalVelocity;
    public bool IsGrounded => characterController != null && characterController.isGrounded;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Start()
    {
        mouseSensitivity = SettingsData.GetMouseSensitivity();
        EnablePlayerControl();
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        Vector2 lookInput = inputHandler.RotationInput;

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);

        if (cameraTarget != null)
        {
            cameraTarget.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }

    private void HandleMovement()
    {
        Vector2 moveInput = inputHandler.MovementInput;

        Vector3 moveDirection =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        bool isMoving = moveDirection.magnitude > 0.1f;
        float targetSpeed = inputHandler.SprintHeld ? sprintSpeed : walkSpeed;

        currentMoveSpeed = isMoving ? targetSpeed : 0f;
        normalizedMoveSpeed = isMoving ? currentMoveSpeed / sprintSpeed : 0f;

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        if (characterController.isGrounded && inputHandler.ConsumeJumpInput())
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = moveDirection * currentMoveSpeed;
        velocity.y = verticalVelocity;

        characterController.Move(velocity * Time.deltaTime);
    }

    public void EnablePlayerControl()
    {
        inputHandler.SetGameplayInputEnabled(true);
        inputHandler.ClearOneFrameInputs();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DisablePlayerControl()
    {
        inputHandler.SetGameplayInputEnabled(false);
        inputHandler.ClearOneFrameInputs();

        currentMoveSpeed = 0f;
        normalizedMoveSpeed = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetMouseSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
    }
}