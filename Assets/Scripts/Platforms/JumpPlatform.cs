using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] private float jumpForce = 17;
    [SerializeField] private AudioSource PlatformSoundEffect;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        MakeNonSticky();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 hitPosition = collision.contacts[0].normal;
            if (hitPosition.y <= -0.5) 
            {
                Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

                if (playerRigidbody != null)
                {
                    animator.SetTrigger("IsPushing");
                    PlatformSoundEffect.Play();

                    if (playerRigidbody != null)
                    {
                        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
                    }

                    animator.SetTrigger("IsIdle");
                }
            }

        }
    }

    
    private void MakeNonSticky()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider.sharedMaterial != null)
        {
            PhysicsMaterial2D newMaterial = new PhysicsMaterial2D();

            newMaterial.friction = 0f;

            boxCollider.sharedMaterial = newMaterial;
        }
        else
        {
            boxCollider.sharedMaterial = new PhysicsMaterial2D { friction = 0f };
        }
    }

}
