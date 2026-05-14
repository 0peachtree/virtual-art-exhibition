using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private bool playOnStart = true;

    [Header("Audio Settings")]
    [SerializeField] private float defaultVolume = 0.5f;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        SetupAudioSource();
    }

    private void Start()
    {
        float savedVolume = SettingsData.GetVolume();

        SetVolume(savedVolume);

        if (playOnStart)
        {
            PlayMusic();
        }
    }

    private void SetupAudioSource()
    {
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = defaultVolume;
        audioSource.mute = false;
    }

    public void PlayMusic()
    {
        if (backgroundMusic == null)
        {
            Debug.LogWarning("MusicManager has no background music assigned.");
            return;
        }

        if (audioSource.clip == null)
        {
            audioSource.clip = backgroundMusic;
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        float clampedVolume = Mathf.Clamp01(volume);

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        audioSource.volume = clampedVolume;
        AudioListener.volume = clampedVolume;
    }

    public float GetCurrentVolume()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        return audioSource.volume;
    }
}