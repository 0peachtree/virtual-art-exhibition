using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [Header("Scene Names")]
    [SerializeField] private string pauseMenuSceneName = "PauseMenuScene";
    [SerializeField] private string settingsMenuSceneName = "SettingsMenuScene";

    [Header("Player References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private CrosshairUI crosshairUI;

    private bool isPaused;
    private bool pauseSceneLoaded;

    public bool IsPaused => isPaused;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleEscapeInput();
        }
    }

    private void HandleEscapeInput()
    {
        // Do not resume the game while SettingsMenuScene is open.
        // SettingsMenu.cs will handle closing itself.
        if (IsSceneLoaded(settingsMenuSceneName))
        {
            return;
        }

        // Do not pause while the painting info panel is open.
        if (MuseumInteractionManager.Instance != null &&
            MuseumInteractionManager.Instance.InteractionActive)
        {
            return;
        }

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused)
            return;

        isPaused = true;

        Time.timeScale = 0f;

        if (playerController != null)
        {
            playerController.DisablePlayerControl();
        }

        if (playerInteract != null)
        {
            playerInteract.SetInteractionDetectionEnabled(false);
            playerInteract.ClearCurrentPainting();
        }

        if (crosshairUI != null)
        {
            crosshairUI.Hide();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (!pauseSceneLoaded)
        {
            SceneManager.LoadScene(pauseMenuSceneName, LoadSceneMode.Additive);
            pauseSceneLoaded = true;
        }
    }

    public void ResumeGame()
    {
        if (!isPaused)
            return;

        // Safety check: do not resume while settings is open.
        if (IsSceneLoaded(settingsMenuSceneName))
            return;

        isPaused = false;

        Time.timeScale = 1f;

        if (pauseSceneLoaded)
        {
            SceneManager.UnloadSceneAsync(pauseMenuSceneName);
            pauseSceneLoaded = false;
        }

        if (playerController != null)
        {
            playerController.EnablePlayerControl();
        }

        if (playerInteract != null)
        {
            playerInteract.SetInteractionDetectionEnabled(true);
            playerInteract.ClearCurrentPainting();
        }

        if (crosshairUI != null)
        {
            crosshairUI.Show();
            crosshairUI.SetInteractable(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MarkPauseSceneUnloaded()
    {
        pauseSceneLoaded = false;
    }

    private bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
            {
                return true;
            }
        }

        return false;
    }
}