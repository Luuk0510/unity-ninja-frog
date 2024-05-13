using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance = null;
    public AudioSource audioSource;
    public AudioClip[] levelMusic;

    public static BackgroundMusic Instance => instance;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        audioSource = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int levelIndex = scene.buildIndex;

        if (levelIndex >= 0 && levelIndex < levelMusic.Length)
        {
            AudioClip thisLevelMusic = levelMusic[levelIndex];
            if (thisLevelMusic && thisLevelMusic != audioSource.clip)
            {
                audioSource.clip = thisLevelMusic;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    public void RestartMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
