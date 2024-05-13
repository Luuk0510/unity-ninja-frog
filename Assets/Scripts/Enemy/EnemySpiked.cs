using System.Collections;
using UnityEngine;

public class EnemySpiked : MonoBehaviour
{
    [SerializeField] private Transform player;

    private bool isFalling = false;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        StartCoroutine(BlinkingEffect());
    }

    private void Update()
    {
        CheckForPlayerBelow();
    }

    private void CheckForPlayerBelow()
    {
        // Check if player is horizontally aligned with the enemy and also if the player is below the enemy
        if (Mathf.Abs(transform.position.x - player.position.x) < 0.5f // Adjust this value as needed for alignment tolerance
            && player.position.y < transform.position.y // Check if the player is below
            && !isFalling)
        {
            Fall();
        }
    }

    private void Fall()
    {
        rb.isKinematic = false; // Allow the enemy to fall
        rb.gravityScale = 2;    // Adjust this as needed
        isFalling = true;
        animator.SetTrigger("IsFalling"); // Trigger the falling animation
    }

    private IEnumerator BlinkingEffect()
    {
        while (!isFalling)
        {
            yield return new WaitForSeconds(3); // Time before starting the blinking effect
            animator.SetTrigger("IsBlinking");  // Trigger the blinking animation
            yield return new WaitForSeconds(0.5f); // Duration of the blinking effect
            animator.SetTrigger("IsIdle"); // Return to idle animation
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();
            if (playerLife != null)
            {
                playerLife.Die(); // The player dies upon collision with the enemy
            }
        }
        // Add more collision behavior if needed
    }
}
