using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static class FruitManager
    {
        public static HashSet<string> CollectedFruit = new HashSet<string>();
    }

    public static GameManager Instance { get; private set; }
    public int FruitCount { get; private set; }
    public float RemainingTime { get; private set; }
    public int Score { get; private set; }
    public int CurrentLevel { get; set; }
    public int NextLevel { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // You can remove this line if OnSceneLoaded is no longer needed.
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Since we're no longer using different times per level, this can be simplified or removed.
        // Just keep the part that disables collected fruits if needed.
        Instance.DisableCollectedFruits();
    }


    public void LoadNextLevel()
    {
        ResetLevel();
        SceneManager.LoadScene(NextLevel);
    }

    public void GetCurrentLevelIndex()
    {
        CurrentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void RetryLevel()
    {
        ResetLevel();
        SceneManager.LoadScene(CurrentLevel);
    }

    public void StartMenu()
    {
        ResetLevel();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void UpdateScoreAndTimeAndLevel(int fruit, float time, int level, int nextLevel)
    {
        FruitCount = fruit * 5;
        RemainingTime = (int)time;
        Score = FruitCount + (int)RemainingTime;
        int score = FruitCount + (int)RemainingTime;
        NextLevel = nextLevel;
        HighScoreManager.Instance.SaveScore($"Level{level}Score", score);
        ResetLevel();
    }

    public void ResetLevel()
    {
        Checkpoint.ResetCheckpoints();
        FruitManager.CollectedFruit.Clear();
        BackgroundMusic.instance.RestartMusic();
        TimerManager.instance.ResetTimer();
    }

    public void DisableCollectedFruits()
    {
        foreach (string fruitName in FruitManager.CollectedFruit)
        {
            GameObject fruit = GameObject.Find(fruitName);
            if (fruit != null)
            {
                fruit.SetActive(false);
            }
        }
    }

}
