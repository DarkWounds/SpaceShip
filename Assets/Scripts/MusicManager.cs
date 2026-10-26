using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    public AudioClip backgroundMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPaused(bool paused)
    {
        if (audioSource == null) return;

        if (paused) audioSource.Pause();
        else audioSource.UnPause();
    }

    public static MusicManager Instance => instance;
}
