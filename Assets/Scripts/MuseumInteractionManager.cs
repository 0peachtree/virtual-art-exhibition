using UnityEngine;

public class MuseumInteractionManager : MonoBehaviour
{
    public static MuseumInteractionManager Instance { get; private set; }

    [Header("Managers")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraZoomManager cameraZoomManager;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerInteract playerInteract;

    private Painting activePainting;
    private bool interactionActive;

    public bool InteractionActive => interactionActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartPaintingInteraction(Painting painting)
    {
        if (painting == null)
            return;

        if (interactionActive)
            return;

        activePainting = painting;
        interactionActive = true;

        if (playerInteract != null)
        {
            playerInteract.SetInteractionDetectionEnabled(false);
            playerInteract.ClearCurrentPainting();
        }

        if (playerController != null)
        {
            playerController.DisablePlayerControl();
        }

        if (cameraZoomManager != null)
        {
            cameraZoomManager.ZoomToPainting(painting);
        }

        if (uiManager != null)
        {
            uiManager.OpenPaintingInfo(painting);
        }
    }

    public void ClosePaintingInteraction()
    {
        if (!interactionActive)
            return;

        if (uiManager != null)
        {
            uiManager.ClosePaintingInfo();
        }

        if (cameraZoomManager != null)
        {
            cameraZoomManager.ReturnToPlayerCamera();
        }

        if (activePainting != null)
        {
            activePainting.SetHighlighted(false);
        }

        activePainting = null;
        interactionActive = false;

        if (playerController != null)
        {
            playerController.EnablePlayerControl();
        }

        if (playerInteract != null)
        {
            playerInteract.SetInteractionDetectionEnabled(true);
            playerInteract.ClearCurrentPainting();
        }
    }
}