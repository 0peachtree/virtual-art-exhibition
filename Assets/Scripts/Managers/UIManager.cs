using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Painting Info Panel")]
    [SerializeField] private UIFade paintingInfoPanelFade;

    [Header("Painting Overview Image")]
    [SerializeField] private RawImage paintingImage;
    [SerializeField] private AspectRatioFitter paintingImageAspectFitter;
    [SerializeField] private AspectRatioFitter paintingImageFrameAspectFitter;

    [Header("Painting Overview Size")]
    [SerializeField] private RectTransform paintingImageFrameRect;
    [SerializeField] private RectTransform paintingImageRect;
    [SerializeField] private Vector2 maxFrameSize = new Vector2(300f, 200f);
    [SerializeField] private Vector2 minFrameSize = new Vector2(140f, 90f);
    [SerializeField] private float imagePadding = 8f;

    [Header("Painting Text")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text artistText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private ScrollRect descriptionScrollRect;

    [Header("Crosshair")]
    [SerializeField] private CrosshairUI crosshairUI;

    private void Start()
    {
        ClosePaintingInfoInstant();
    }

    public void OpenPaintingInfo(Painting painting)
    {
        if (painting == null)
            return;

        UpdatePaintingImage(painting.PaintingImage);
        UpdatePaintingText(painting);
        ResetDescriptionScroll();

        if (crosshairUI != null)
        {
            crosshairUI.Hide();
        }

        if (paintingInfoPanelFade != null)
        {
            paintingInfoPanelFade.gameObject.SetActive(true);
            paintingInfoPanelFade.Show();
        }
    }

    private void UpdatePaintingImage(Texture2D texture)
    {
        if (paintingImage == null)
            return;

        paintingImage.texture = texture;

        if (texture == null)
            return;

        float aspectRatio = (float)texture.width / texture.height;

        Vector2 fittedFrameSize = GetFittedSize(
            aspectRatio,
            maxFrameSize,
            minFrameSize
        );

        if (paintingImageFrameRect != null)
        {
            paintingImageFrameRect.sizeDelta = fittedFrameSize;
        }

        Vector2 imageMaxSize = new Vector2(
            Mathf.Max(1f, fittedFrameSize.x - imagePadding * 2f),
            Mathf.Max(1f, fittedFrameSize.y - imagePadding * 2f)
        );

        Vector2 imageMinSize = new Vector2(
            Mathf.Max(1f, minFrameSize.x - imagePadding * 2f),
            Mathf.Max(1f, minFrameSize.y - imagePadding * 2f)
        );

        Vector2 fittedImageSize = GetFittedSize(
            aspectRatio,
            imageMaxSize,
            imageMinSize
        );

        if (paintingImageRect != null)
        {
            paintingImageRect.sizeDelta = fittedImageSize;
        }

        if (paintingImageAspectFitter != null)
        {
            paintingImageAspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            paintingImageAspectFitter.aspectRatio = aspectRatio;
        }

        if (paintingImageFrameAspectFitter != null)
        {
            paintingImageFrameAspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            paintingImageFrameAspectFitter.aspectRatio = aspectRatio;
        }
    }

    private Vector2 GetFittedSize(float aspectRatio, Vector2 maxSize, Vector2 minSize)
    {
        float width = maxSize.x;
        float height = width / aspectRatio;

        if (height > maxSize.y)
        {
            height = maxSize.y;
            width = height * aspectRatio;
        }

        width = Mathf.Max(width, minSize.x);
        height = Mathf.Max(height, minSize.y);

        width = Mathf.Min(width, maxSize.x);
        height = Mathf.Min(height, maxSize.y);

        return new Vector2(width, height);
    }

    private void UpdatePaintingText(Painting painting)
    {
        if (titleText != null)
        {
            titleText.text = string.IsNullOrWhiteSpace(painting.PaintingTitle)
                ? "Untitled Artwork"
                : painting.PaintingTitle;
        }

        if (artistText != null)
        {
            artistText.text = string.IsNullOrWhiteSpace(painting.ArtistName)
                ? "by Unknown Artist"
                : $"by {painting.ArtistName}";
        }

        if (descriptionText != null)
        {
            descriptionText.text = string.IsNullOrWhiteSpace(painting.PaintingDescription)
                ? "No description available for this artwork."
                : painting.PaintingDescription;
        }
    }

    private void ResetDescriptionScroll()
    {
        if (descriptionScrollRect == null)
            return;

        Canvas.ForceUpdateCanvases();
        descriptionScrollRect.verticalNormalizedPosition = 1f;
    }

    public void ClosePaintingInfo()
    {
        if (paintingInfoPanelFade != null)
        {
            paintingInfoPanelFade.Hide();
        }

        if (crosshairUI != null)
        {
            crosshairUI.Show();
            crosshairUI.SetInteractable(false);
        }
    }

    private void ClosePaintingInfoInstant()
    {
        if (paintingInfoPanelFade != null)
        {
            CanvasGroup canvasGroup = paintingInfoPanelFade.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            paintingInfoPanelFade.gameObject.SetActive(false);
        }

        if (crosshairUI != null)
        {
            crosshairUI.Show();
            crosshairUI.SetInteractable(false);
        }
    }
}