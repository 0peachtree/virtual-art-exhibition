using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string museumSceneName = "MuseumScene";
    [SerializeField] private string settingsSceneName = "SettingsMenuScene";

    private void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SettingsData.ApplyAllSettings();

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMusic();
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;

        if (IsSceneLoaded(settingsSceneName))
        {
            SceneManager.UnloadSceneAsync(settingsSceneName);
        }

        SceneManager.LoadScene(museumSceneName);
    }

    public void OpenSettings()
    {
        PlayerPrefs.SetString("PreviousMenuScene", "MainMenuScene");

        if (IsSceneLoaded(settingsSceneName))
            return;

        SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Additive);
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