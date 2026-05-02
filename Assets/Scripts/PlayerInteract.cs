using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerInteract : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 4f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("UI")]
    [SerializeField] private CrosshairUI crosshairUI;

    private PlayerInputHandler inputHandler;
    private Painting currentPainting;

    private bool interactionDetectionEnabled = true;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        if (!interactionDetectionEnabled || !inputHandler.GameplayInputEnabled)
        {
            ClearCurrentPainting();
            return;
        }

        DetectPainting();
        HandleInteractionInput();
    }

    private void DetectPainting()
    {
        Painting detectedPainting = null;

        if (playerCamera == null)
        {
            Debug.LogError("PlayerInteract is missing the Player Camera reference.");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer))
        {
            detectedPainting = hit.collider.GetComponentInParent<Painting>();
        }

        if (detectedPainting != currentPainting)
        {
            if (currentPainting != null)
            {
                currentPainting.SetHighlighted(false);
            }

            currentPainting = detectedPainting;

            if (currentPainting != null)
            {
                currentPainting.SetHighlighted(true);
            }
        }

        if (crosshairUI != null)
        {
            crosshairUI.SetInteractable(currentPainting != null);
        }
    }

    private void HandleInteractionInput()
    {
        if (currentPainting == null)
            return;

        if (inputHandler.ConsumeClickInput())
        {
            currentPainting.Interact();
        }
    }

    public void SetInteractionDetectionEnabled(bool enabled)
    {
        interactionDetectionEnabled = enabled;

        if (!enabled)
        {
            ClearCurrentPainting();
        }
    }

    public void ClearCurrentPainting()
    {
        if (currentPainting != null)
        {
            currentPainting.SetHighlighted(false);
            currentPainting = null;
        }

        if (crosshairUI != null)
        {
            crosshairUI.SetInteractable(false);
        }
    }
}