using System.Collections;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float animationInterval = 2f;
    [SerializeField] private AudioSource audioSource;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        // Check if this powerup was already collected
        if (Checkpoint.CollectedPowerUps.Contains(gameObject.name))
        {
            // Optionally make the powerup invisible or disable it entirely
            gameObject.SetActive(false); // or GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            StartCoroutine(AnimatePowerup());
        }
    }

    private IEnumerator AnimatePowerup()
    {
        while (true)
        {
            animator.SetTrigger("IsMoving");
            animator.SetTrigger("IsIdle");
            yield return new WaitForSeconds(animationInterval);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            animator.SetTrigger("IsCollected");

            PlayerMovement playerMovement = collider.gameObject.GetComponent<PlayerMovement>();

            if (gameObject.CompareTag("DoubleJump"))
            {
                playerMovement.EnableDoubleJump();
                Checkpoint.CollectedPowerUps.Add("DoubleJump:" + gameObject.name);
            }
            else if (gameObject.CompareTag("WallJump"))
            {
                playerMovement.EnableWallJump();
                Checkpoint.CollectedPowerUps.Add("WallJump:" + gameObject.name);
            }

            Destroy(gameObject, 0.8f); // Destroy after collection animation
        }
    }


}
