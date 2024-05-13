using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHighScore : MonoBehaviour
{
    public List<Text> highScoreTexts; // Assign in editor, make sure it matches number of levels

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DisplayHighScores();
    }

    private void DisplayHighScores()
    {
        bool hasAssignedTexts = highScoreTexts.Exists(text => text != null);
        if (!hasAssignedTexts) return;

        for (int i = 0; i < highScoreTexts.Count; i++)
        {
            if (highScoreTexts[i] == null) continue;

            string levelKey = $"Level{i + 1}Score";
            int highScore = HighScoreManager.Instance.GetHighScore(levelKey);
            highScoreTexts[i].text = $"Level {i+1}: {highScore}";
        }
    }

}
