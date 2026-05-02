using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls controls;

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
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();

        controls.Player.Movement.performed += OnMovementPerformed;
        controls.Player.Movement.canceled += OnMovementCanceled;

        controls.Player.Rotation.performed += OnRotationPerformed;
        controls.Player.Rotation.canceled += OnRotationCanceled;

        controls.Player.Jump.performed += OnJumpPerformed;

        controls.Player.Sprint.performed += OnSprintPerformed;
        controls.Player.Sprint.canceled += OnSprintCanceled;

        controls.Player.Click.performed += OnClickPerformed;
    }

    private void OnDisable()
    {
        controls.Player.Movement.performed -= OnMovementPerformed;
        controls.Player.Movement.canceled -= OnMovementCanceled;

        controls.Player.Rotation.performed -= OnRotationPerformed;
        controls.Player.Rotation.canceled -= OnRotationCanceled;

        controls.Player.Jump.performed -= OnJumpPerformed;

        controls.Player.Sprint.performed -= OnSprintPerformed;
        controls.Player.Sprint.canceled -= OnSprintCanceled;

        controls.Player.Click.performed -= OnClickPerformed;

        controls.Player.Disable();
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
        if (!gameplayInputEnabled)
            return;

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
        {
            jumpPressed = false;
            return false;
        }

        bool wasPressed = jumpPressed;
        jumpPressed = false;
        return wasPressed;
    }

    public bool ConsumeClickInput()
    {
        if (!gameplayInputEnabled)
        {
            clickPressed = false;
            return false;
        }

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
            jumpPressed = false;
            sprintHeld = false;
            clickPressed = false;
        }
    }

    public void ClearOneFrameInputs()
    {
        jumpPressed = false;
        clickPressed = false;
    }
}