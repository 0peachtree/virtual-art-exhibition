using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls playerControls;

    private Vector2 movementInput;
    private Vector2 rotationInput;

    private bool jumpPressed;
    private bool sprintHeld;
    private bool clickPressed;

    private bool gameplayInputEnabled = true;

    public Vector2 MovementInput => gameplayInputEnabled ? movementInput : Vector2.zero;
    public Vector2 RotationInput => gameplayInputEnabled ? rotationInput : Vector2.zero;
    public bool SprintHeld => gameplayInputEnabled && sprintHeld;
    public bool GameplayInputEnabled => gameplayInputEnabled;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Movement.performed += OnMovementPerformed;
        playerControls.Player.Movement.canceled += OnMovementCanceled;

        playerControls.Player.Rotation.performed += OnRotationPerformed;
        playerControls.Player.Rotation.canceled += OnRotationCanceled;

        playerControls.Player.Jump.performed += OnJumpPerformed;

        playerControls.Player.Sprint.performed += OnSprintPerformed;
        playerControls.Player.Sprint.canceled += OnSprintCanceled;

        playerControls.Player.Click.performed += OnClickPerformed;
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
    }

    private void OnRotationPerformed(InputAction.CallbackContext context)
    {
        rotationInput = context.ReadValue<Vector2>();
    }

    private void OnRotationCanceled(InputAction.CallbackContext context)
    {
        rotationInput = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (!gameplayInputEnabled)
            return;

        jumpPressed = true;
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        sprintHeld = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        sprintHeld = false;
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        if (!gameplayInputEnabled)
            return;

        clickPressed = true;
    }

    public bool ConsumeJumpInput()
    {
        if (!gameplayInputEnabled)
            return false;

        bool wasPressed = jumpPressed;
        jumpPressed = false;
        return wasPressed;
    }

    public bool ConsumeClickInput()
    {
        if (!gameplayInputEnabled)
            return false;

        bool wasPressed = clickPressed;
        clickPressed = false;
        return wasPressed;
    }

    public void SetGameplayInputEnabled(bool enabled)
    {
        gameplayInputEnabled = enabled;

        if (!enabled)
        {
            movementInput = Vector2.zero;
            rotationInput = Vector2.zero;
            sprintHeld = false;
            ClearOneFrameInputs();
        }
    }

    public void ClearOneFrameInputs()
    {
        jumpPressed = false;
        clickPressed = false;
    }
}