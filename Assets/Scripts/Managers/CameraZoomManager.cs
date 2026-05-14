using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CameraZoomManager : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera paintingZoomCamera;

    [Header("Priorities")]
    [SerializeField] private int playerNormalPriority = 20;
    [SerializeField] private int paintingInactivePriority = 5;
    [SerializeField] private int paintingActivePriority = 30;

    [Header("Blend Timing")]
    [SerializeField] private float cameraBlendTime = 0.5f;

    [Header("Painting Zoom Look")]
    [SerializeField] private bool allowLookAroundPainting = true;
    [SerializeField] private bool requireRightMouseButton = true;
    [SerializeField] private float zoomLookSensitivity = 0.12f;
    [SerializeField] private float maxYawAngle = 25f;
    [SerializeField] private float minPitchAngle = -18f;
    [SerializeField] private float maxPitchAngle = 18f;

    private bool zoomLookActive;
    private Quaternion baseZoomRotation;
    private float currentYaw;
    private float currentPitch;

    public float CameraBlendTime => cameraBlendTime;

    private void Start()
    {
        ReturnToPlayerCamera();
    }

    private void Update()
    {
        HandlePaintingZoomLook();
    }

    public void ZoomToPainting(Painting painting)
    {
        if (painting == null)
            return;

        if (painting.ZoomPoint == null)
        {
            Debug.LogError($"Painting '{painting.name}' does not have a ZoomPoint assigned.");
            return;
        }

        if (playerCamera == null || paintingZoomCamera == null)
        {
            Debug.LogError("CameraZoomManager is missing CinemachineCamera references.");
            return;
        }

        paintingZoomCamera.transform.SetPositionAndRotation(
            painting.ZoomPoint.position,
            painting.ZoomPoint.rotation
        );

        baseZoomRotation = painting.ZoomPoint.rotation;
        currentYaw = 0f;
        currentPitch = 0f;
        zoomLookActive = true;

        playerCamera.Priority = playerNormalPriority;
        paintingZoomCamera.Priority = paintingActivePriority;
    }

    public void ReturnToPlayerCamera()
    {
        zoomLookActive = false;
        currentYaw = 0f;
        currentPitch = 0f;

        if (playerCamera != null)
        {
            playerCamera.Priority = playerNormalPriority;
        }

        if (paintingZoomCamera != null)
        {
            paintingZoomCamera.Priority = paintingInactivePriority;
        }
    }

    private void HandlePaintingZoomLook()
    {
        if (!zoomLookActive || !allowLookAroundPainting)
            return;

        if (paintingZoomCamera == null || Mouse.current == null)
            return;

        if (requireRightMouseButton && !Mouse.current.rightButton.isPressed)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        currentYaw += mouseDelta.x * zoomLookSensitivity;
        currentPitch -= mouseDelta.y * zoomLookSensitivity;

        currentYaw = Mathf.Clamp(currentYaw, -maxYawAngle, maxYawAngle);
        currentPitch = Mathf.Clamp(currentPitch, minPitchAngle, maxPitchAngle);

        Quaternion lookOffset = Quaternion.Euler(currentPitch, currentYaw, 0f);
        paintingZoomCamera.transform.rotation = baseZoomRotation * lookOffset;
    }
}