using UnityEngine;

public class RollTrigger : MonoBehaviour
{
    public RollingBall rollingBall; // Assign this in the Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Make sure your player GameObject has the tag "Player"
        {
            rollingBall.StartRolling();
        }
    }
}
