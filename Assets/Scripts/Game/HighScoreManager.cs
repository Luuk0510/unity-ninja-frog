using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SaveScore(string levelKey, int score)
    {
        int highScore = PlayerPrefs.GetInt(levelKey, 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt(levelKey, score);
            PlayerPrefs.Save();
        }
        int scores = GetHighScore(levelKey);
        Debug.Log(scores);
    }

    public int GetHighScore(string levelKey)
    {
        return PlayerPrefs.GetInt(levelKey, 0);
    }
}
