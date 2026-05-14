using UnityEngine;

public static class SettingsData
{
    private const string VolumeKey = "Settings_Volume";
    private const string SensitivityKey = "Settings_MouseSensitivity";
    private const string FullscreenKey = "Settings_Fullscreen";

    private const float DefaultVolume = 0.5f;
    private const float DefaultSensitivity = 0.12f;
    private const int DefaultFullscreen = 1;

    public static float GetVolume()
    {
        if (!PlayerPrefs.HasKey(VolumeKey))
        {
            PlayerPrefs.SetFloat(VolumeKey, DefaultVolume);
            PlayerPrefs.Save();
            return DefaultVolume;
        }

        return PlayerPrefs.GetFloat(VolumeKey, DefaultVolume);
    }

    public static void SetVolume(float value)
    {
        float clampedValue = Mathf.Clamp01(value);

        PlayerPrefs.SetFloat(VolumeKey, clampedValue);
        AudioListener.volume = clampedValue;

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(clampedValue);
            MusicManager.Instance.PlayMusic();
        }
    }

    public static float GetMouseSensitivity()
    {
        if (!PlayerPrefs.HasKey(SensitivityKey))
        {
            PlayerPrefs.SetFloat(SensitivityKey, DefaultSensitivity);
            PlayerPrefs.Save();
            return DefaultSensitivity;
        }

        return PlayerPrefs.GetFloat(SensitivityKey, DefaultSensitivity);
    }

    public static void SetMouseSensitivity(float value)
    {
        float clampedValue = Mathf.Clamp(value, 0.02f, 0.5f);
        PlayerPrefs.SetFloat(SensitivityKey, clampedValue);
    }

    public static bool GetFullscreen()
    {
        if (!PlayerPrefs.HasKey(FullscreenKey))
        {
            PlayerPrefs.SetInt(FullscreenKey, DefaultFullscreen);
            PlayerPrefs.Save();
            return true;
        }

        return PlayerPrefs.GetInt(FullscreenKey, DefaultFullscreen) == 1;
    }

    public static void SetFullscreen(bool value)
    {
        PlayerPrefs.SetInt(FullscreenKey, value ? 1 : 0);
        Screen.fullScreen = value;
    }

    public static void ApplyAllSettings()
    {
        float volume = GetVolume();

        AudioListener.volume = volume;

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(volume);
            MusicManager.Instance.PlayMusic();
        }

        Screen.fullScreen = GetFullscreen();
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static void ResetSettings()
    {
        PlayerPrefs.DeleteKey(VolumeKey);
        PlayerPrefs.DeleteKey(SensitivityKey);
        PlayerPrefs.DeleteKey(FullscreenKey);
        PlayerPrefs.Save();

        ApplyAllSettings();
    }
}