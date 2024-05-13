using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private Transform playerParentBeforeCollision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Make sure this is the tag your player has.
        {
            if (collision.relativeVelocity.y <= 0 && Mathf.Abs(collision.contacts[0].normal.y) >= 0.7f)
            {
                // Store the player's original parent before changing it.
                playerParentBeforeCollision = collision.transform.parent;
                collision.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Again, check the player specifically.
        {
            // Only reset the parent if it's currently set to this platform to avoid overriding other parenting logic.
            if (collision.transform.parent == transform)
            {
                collision.transform.SetParent(playerParentBeforeCollision);
            }
        }
    }
}
