using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Painting Info UI")]
    [SerializeField] private GameObject dimBackground;
    [SerializeField] private UIFade paintingInfoPanelFade;
    [SerializeField] private RawImage paintingImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

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

        if (dimBackground != null)
        {
            dimBackground.SetActive(true);
        }

        if (paintingImage != null)
        {
            paintingImage.texture = painting.PaintingImage;
        }

        if (titleText != null)
        {
            titleText.text = painting.PaintingTitle;
        }

        if (descriptionText != null)
        {
            descriptionText.text = painting.PaintingDescription;
        }

        if (crosshairUI != null)
        {
            crosshairUI.Hide();
        }

        if (paintingInfoPanelFade != null)
        {
            paintingInfoPanelFade.Show();
        }
    }

    public void ClosePaintingInfo()
    {
        if (paintingInfoPanelFade != null)
        {
            paintingInfoPanelFade.Hide();
        }

        if (dimBackground != null)
        {
            dimBackground.SetActive(false);
        }

        if (crosshairUI != null)
        {
            crosshairUI.Show();
            crosshairUI.SetInteractable(false);
        }
    }

    private void ClosePaintingInfoInstant()
    {
        if (dimBackground != null)
        {
            dimBackground.SetActive(false);
        }

        if (crosshairUI != null)
        {
            crosshairUI.Show();
            crosshairUI.SetInteractable(false);
        }
    }
}