using UnityEngine;
using UnityEngine.UI;

public class LevelSummary : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private void Start()
    {
        if (GameManager.Instance != null && scoreText != null)
        {
            scoreText.text = $"Fruit score: {GameManager.Instance.FruitCount} \nOvergebleven tijd: {GameManager.Instance.RemainingTime} \nTotale score: {GameManager.Instance.Score}";
        }
    }
}
