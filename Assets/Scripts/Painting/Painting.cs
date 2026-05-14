using UnityEngine;

public class Painting : MonoBehaviour
{
    public enum PaintingOrientation
    {
        Landscape,
        Portrait,
        Square
    }

    [Header("Painting Data")]
    [SerializeField] private string paintingTitle;
    [SerializeField] private string artistName;

    [TextArea(4, 10)]
    [SerializeField] private string paintingDescription;

    [SerializeField] private Texture2D paintingImage;

    [Header("Painting Orientation")]
    [SerializeField] private PaintingOrientation orientation = PaintingOrientation.Landscape;

    [Header("Frame Settings")]
    [SerializeField] private float frameThickness = 0.15f;
    [SerializeField] private float artworkDepth = 0.04f;
    [SerializeField] private float frameDepth = 0.12f;

    [Header("Prefab Parts")]
    [SerializeField] private Transform artworkMesh;
    [SerializeField] private Transform frameTop;
    [SerializeField] private Transform frameBottom;
    [SerializeField] private Transform frameLeft;
    [SerializeField] private Transform frameRight;
    [SerializeField] private BoxCollider boxCollider;

    [Header("Camera")]
    [SerializeField] private Transform zoomPoint;
    [SerializeField] private float zoomDistance = 2f;

    public string PaintingTitle => paintingTitle;
    public string ArtistName => artistName;
    public string PaintingDescription => paintingDescription;
    public Texture2D PaintingImage => paintingImage;
    public Transform ZoomPoint => zoomPoint;

    private void Reset()
    {
        boxCollider = GetComponent<BoxCollider>();
        AutoFindParts();
        ApplyFrameSize();
    }

    private void OnValidate()
    {
        AutoFindParts();
        ApplyFrameSize();
    }

    private void AutoFindParts()
    {
        if (artworkMesh == null)
            artworkMesh = transform.Find("ArtworkMesh");

        if (frameTop == null)
            frameTop = transform.Find("Frame_Top");

        if (frameBottom == null)
            frameBottom = transform.Find("Frame_Bottom");

        if (frameLeft == null)
            frameLeft = transform.Find("Frame_Left");

        if (frameRight == null)
            frameRight = transform.Find("Frame_Right");

        if (zoomPoint == null)
            zoomPoint = transform.Find("ZoomPoint");

        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();
    }

    public void ApplyFrameSize()
    {
        Vector2 artworkSize = GetArtworkSize();

        float artworkWidth = artworkSize.x;
        float artworkHeight = artworkSize.y;

        float totalWidth = artworkWidth + frameThickness * 2f;
        float totalHeight = artworkHeight + frameThickness * 2f;

        ResizeArtwork(artworkWidth, artworkHeight);
        ResizeFrames(artworkWidth, artworkHeight, totalWidth, totalHeight);
        ResizeCollider(totalWidth, totalHeight);
        PositionZoomPoint();
    }

    private Vector2 GetArtworkSize()
    {
        switch (orientation)
        {
            case PaintingOrientation.Landscape:
                return new Vector2(2.4f, 1.5f);

            case PaintingOrientation.Portrait:
                return new Vector2(1.5f, 2.4f);

            case PaintingOrientation.Square:
                return new Vector2(1.8f, 1.8f);

            default:
                return new Vector2(2.4f, 1.5f);
        }
    }

    private void ResizeArtwork(float artworkWidth, float artworkHeight)
    {
        if (artworkMesh == null)
            return;

        artworkMesh.localPosition = Vector3.zero;

        // If your painting texture is upside down, keep this.
        // If your texture is already correct, change this to Quaternion.identity.
        artworkMesh.localRotation = Quaternion.Euler(0f, 0f, 180f);

        artworkMesh.localScale = new Vector3(artworkWidth, artworkHeight, artworkDepth);
    }

    private void ResizeFrames(float artworkWidth, float artworkHeight, float totalWidth, float totalHeight)
    {
        if (frameTop != null)
        {
            frameTop.localPosition = new Vector3(
                0f,
                artworkHeight / 2f + frameThickness / 2f,
                -0.02f
            );

            frameTop.localRotation = Quaternion.identity;
            frameTop.localScale = new Vector3(totalWidth, frameThickness, frameDepth);
        }

        if (frameBottom != null)
        {
            frameBottom.localPosition = new Vector3(
                0f,
                -artworkHeight / 2f - frameThickness / 2f,
                -0.02f
            );

            frameBottom.localRotation = Quaternion.identity;
            frameBottom.localScale = new Vector3(totalWidth, frameThickness, frameDepth);
        }

        if (frameLeft != null)
        {
            frameLeft.localPosition = new Vector3(
                -artworkWidth / 2f - frameThickness / 2f,
                0f,
                -0.02f
            );

            frameLeft.localRotation = Quaternion.identity;
            frameLeft.localScale = new Vector3(frameThickness, totalHeight, frameDepth);
        }

        if (frameRight != null)
        {
            frameRight.localPosition = new Vector3(
                artworkWidth / 2f + frameThickness / 2f,
                0f,
                -0.02f
            );

            frameRight.localRotation = Quaternion.identity;
            frameRight.localScale = new Vector3(frameThickness, totalHeight, frameDepth);
        }
    }

    private void ResizeCollider(float totalWidth, float totalHeight)
    {
        if (boxCollider == null)
            return;

        boxCollider.center = Vector3.zero;
        boxCollider.size = new Vector3(totalWidth, totalHeight, 0.25f);
    }

    private void PositionZoomPoint()
    {
        if (zoomPoint == null)
            return;

        zoomPoint.localPosition = new Vector3(0f, 0f, -zoomDistance);
        zoomPoint.localScale = Vector3.one;
    }

    public void Interact()
{
    if (MuseumInteractionManager.Instance == null)
    {
        Debug.LogError("MuseumInteractionManager was not found in the scene.");
        return;
    }

    MuseumInteractionManager.Instance.StartPaintingInteraction(this);
}
}