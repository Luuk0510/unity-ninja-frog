using UnityEngine;

public class FanPlatform : MonoBehaviour
{
    public float liftForce = 40f;
    public float maxHoverHeight = 2f; // Maximum height above the fan where the lift force is applied
    public float maxUpwardSpeed = 2f; // Maximum upward speed the player can have before the fan stops applying force

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = other.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                // Calculate the height above the fan
                float heightAboveFan = other.transform.position.y - transform.position.y;

                // Apply force if below max hover height and moving down or stationary
                if (heightAboveFan < maxHoverHeight && playerRigidbody.velocity.y <= maxUpwardSpeed)
                {
                    // Calculate a proportional lift force based on how close the player is to the fan
                    float proportionalHeight = (maxHoverHeight - heightAboveFan) / maxHoverHeight;
                    float lift = liftForce * proportionalHeight;

                    // Apply a modified lift force based on the player's height above the fan
                    playerRigidbody.AddForce(Vector2.up * lift, ForceMode2D.Force);
                }
            }
        }
    }
}
