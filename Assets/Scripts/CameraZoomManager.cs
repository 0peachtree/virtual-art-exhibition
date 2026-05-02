using UnityEngine;
using Unity.Cinemachine;

public class CameraZoomManager : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera paintingZoomCamera;

    [Header("Priorities")]
    [SerializeField] private int playerNormalPriority = 20;
    [SerializeField] private int paintingInactivePriority = 5;
    [SerializeField] private int paintingActivePriority = 30;

    private void Start()
    {
        ReturnToPlayerCamera();
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

        playerCamera.Priority = playerNormalPriority;
        paintingZoomCamera.Priority = paintingActivePriority;
    }

    public void ReturnToPlayerCamera()
    {
        if (playerCamera != null)
        {
            playerCamera.Priority = playerNormalPriority;
        }

        if (paintingZoomCamera != null)
        {
            paintingZoomCamera.Priority = paintingInactivePriority;
        }
    }
}