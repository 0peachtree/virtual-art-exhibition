using System.Collections;
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

    [Header("Timing")]
    [SerializeField] private float uiOpenDelayAfterZoom = 0.35f;
    [SerializeField] private float uiCloseDelayBeforeControl = 0.15f;

    private Painting activePainting;
    private Coroutine interactionRoutine;

    private bool interactionActive;
    private bool isTransitioning;

    public bool InteractionActive => interactionActive || isTransitioning;

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

        if (interactionActive || isTransitioning)
            return;

        if (interactionRoutine != null)
        {
            StopCoroutine(interactionRoutine);
        }

        interactionRoutine = StartCoroutine(StartPaintingInteractionRoutine(painting));
    }

    private IEnumerator StartPaintingInteractionRoutine(Painting painting)
    {
        isTransitioning = true;
        interactionActive = true;
        activePainting = painting;

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

        float delay = uiOpenDelayAfterZoom;

        if (cameraZoomManager != null)
        {
            delay = Mathf.Max(delay, cameraZoomManager.CameraBlendTime * 0.8f);
        }

        yield return new WaitForSecondsRealtime(delay);

        if (uiManager != null)
        {
            uiManager.OpenPaintingInfo(painting);
        }

        isTransitioning = false;
    }

    public void ClosePaintingInteraction()
    {
        if (!interactionActive && !isTransitioning)
            return;

        if (interactionRoutine != null)
        {
            StopCoroutine(interactionRoutine);
            interactionRoutine = null;
        }

        interactionRoutine = StartCoroutine(ClosePaintingInteractionRoutine());
    }

    private IEnumerator ClosePaintingInteractionRoutine()
    {
        isTransitioning = true;

        if (uiManager != null)
        {
            uiManager.ClosePaintingInfo();
        }

        yield return new WaitForSecondsRealtime(uiCloseDelayBeforeControl);

        if (cameraZoomManager != null)
        {
            cameraZoomManager.ReturnToPlayerCamera();
        }

        activePainting = null;
        interactionActive = false;

        float cameraReturnDelay = cameraZoomManager != null
            ? cameraZoomManager.CameraBlendTime * 0.6f
            : 0.25f;

        yield return new WaitForSecondsRealtime(cameraReturnDelay);

        if (playerController != null)
        {
            playerController.EnablePlayerControl();
        }

        if (playerInteract != null)
        {
            playerInteract.SetInteractionDetectionEnabled(true);
            playerInteract.ClearCurrentPainting();
        }

        isTransitioning = false;
    }
}