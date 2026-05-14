using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [Header("Crosshair")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color interactableColor = Color.green;

    private void Awake()
    {
        if (crosshairImage == null)
        {
            crosshairImage = GetComponent<Image>();
        }

        SetInteractable(false);
        Show();
    }

    public void SetInteractable(bool isInteractable)
    {
        if (crosshairImage == null)
            return;

        crosshairImage.color = isInteractable ? interactableColor : normalColor;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}