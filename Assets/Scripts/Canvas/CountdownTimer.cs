using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private Text timerText; // Assign this from the Unity Editor

    private void Update()
    {
        if (TimerManager.instance != null)
        {
            // Subtracting Time.deltaTime each frame to count down
            TimerManager.instance.SubtractTime(Time.deltaTime); // This assumes you handle subtraction inside TimerManager

            // Updating UI
            float time = TimerManager.instance.GetCurrentTime();
            if (time > 0)
            {
                UpdateTimerText(time);
            }
            else
            {
                GameOver();
            }
        }
    }

    private void UpdateTimerText(float time)
    {
        timerText.text = $"Tijd: {Mathf.CeilToInt(time)}";
    }

    private void GameOver()
    {
        GameManager.Instance.GetCurrentLevelIndex();
        GameManager.Instance.NextLevel = 6; // Assuming this is the Game Over level
        BackgroundMusic.instance.RestartMusic();
        GameManager.Instance.LoadNextLevel();
    }
}
