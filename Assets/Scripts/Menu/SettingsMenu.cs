using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Scene Names")]
    [SerializeField] private string defaultBackSceneName = "MainMenuScene";

    private void Start()
    {
        SettingsData.ApplyAllSettings();

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMusic();
        }

        LoadCurrentSettingsIntoUI();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Back();
        }
    }

    private void LoadCurrentSettingsIntoUI()
    {
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.wholeNumbers = false;
            volumeSlider.value = SettingsData.GetVolume();
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = 0.02f;
            sensitivitySlider.maxValue = 0.5f;
            sensitivitySlider.wholeNumbers = false;
            sensitivitySlider.value = SettingsData.GetMouseSensitivity();
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = SettingsData.GetFullscreen();
        }
    }

    public void PreviewVolume(float value)
    {
        SettingsData.SetVolume(value);
        SettingsData.Save();
    }

    public void ApplySettings()
    {
        if (volumeSlider != null)
        {
            SettingsData.SetVolume(volumeSlider.value);
        }

        if (sensitivitySlider != null)
        {
            SettingsData.SetMouseSensitivity(sensitivitySlider.value);
        }

        if (fullscreenToggle != null)
        {
            SettingsData.SetFullscreen(fullscreenToggle.isOn);
        }

        SettingsData.Save();

        PlayerController playerController = FindFirstObjectByType<PlayerController>();

        if (playerController != null)
        {
            playerController.SetMouseSensitivity(SettingsData.GetMouseSensitivity());
        }

        Debug.Log("Settings applied.");
    }

    public void Back()
    {
        ApplySettings();

        string thisSettingsSceneName = gameObject.scene.name;

        if (SceneManager.sceneCount > 1)
        {
            SceneManager.UnloadSceneAsync(thisSettingsSceneName);
            return;
        }

        string previousScene = PlayerPrefs.GetString("PreviousMenuScene", defaultBackSceneName);

        if (string.IsNullOrEmpty(previousScene))
        {
            previousScene = defaultBackSceneName;
        }

        SceneManager.LoadScene(previousScene);
    }

    public void ResetSettings()
    {
        SettingsData.ResetSettings();
        LoadCurrentSettingsIntoUI();
    }
}