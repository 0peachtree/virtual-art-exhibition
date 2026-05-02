using UnityEngine;

public class Painting : MonoBehaviour
{
    [Header("Painting Data")]
    [SerializeField] private string paintingTitle;

    [TextArea(4, 10)]
    [SerializeField] private string paintingDescription;

    [SerializeField] private Texture2D paintingImage;

    [Header("Camera")]
    [SerializeField] private Transform zoomPoint;

    [Header("Highlight")]
    [SerializeField] private Material highlightMaterial;

    private Renderer paintingRenderer;
    private Material originalMaterial;
    private bool isHighlighted;

    public string PaintingTitle => paintingTitle;
    public string PaintingDescription => paintingDescription;
    public Texture2D PaintingImage => paintingImage;
    public Transform ZoomPoint => zoomPoint;

    private void Awake()
    {
        paintingRenderer = GetComponentInChildren<Renderer>();

        if (paintingRenderer != null)
        {
            originalMaterial = paintingRenderer.material;
        }
    }

    public void SetHighlighted(bool highlighted)
    {
        if (paintingRenderer == null || highlightMaterial == null)
            return;

        if (isHighlighted == highlighted)
            return;

        isHighlighted = highlighted;
        paintingRenderer.material = highlighted ? highlightMaterial : originalMaterial;
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