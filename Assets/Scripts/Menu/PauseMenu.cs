using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenuScene";
    [SerializeField] private string settingsSceneName = "SettingsMenuScene";

    public void Resume()
    {
        if (IsSceneLoaded(settingsSceneName))
        {
            Debug.Log("Cannot resume while SettingsMenuScene is open.");
            return;
        }

        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.ResumeGame();
        }
        else
        {
            Debug.LogWarning("PauseManager was not found. Resume only works when PauseMenuScene is loaded from MuseumScene.");
        }
    }

    public void OpenSettings()
    {
        PlayerPrefs.SetString("PreviousMenuScene", "PauseMenuScene");

        if (IsSceneLoaded(settingsSceneName))
            return;

        SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Additive);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.MarkPauseSceneUnloaded();
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
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