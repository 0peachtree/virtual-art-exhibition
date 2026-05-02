using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CloseButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClosePanel);
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(ClosePanel);
        }
    }

    private void ClosePanel()
    {
        if (MuseumInteractionManager.Instance == null)
        {
            Debug.LogError("MuseumInteractionManager was not found in the scene.");
            return;
        }

        MuseumInteractionManager.Instance.ClosePaintingInteraction();
    }
}