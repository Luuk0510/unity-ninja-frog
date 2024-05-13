using UnityEngine;

public class EnemyGhost : MonoBehaviour
{
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private float followDistance = 10f;
    [SerializeField] private float followDelay = 1.5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private AudioSource AudioSource;

    public bool IsDefeated { get; private set; } = false;
     
    private Vector2 targetPosition;
    private GameObject player;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool hasStartedFollowing = false;
    private bool isFollowingTriggered = false;
    private bool facingLeft = true;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        targetPosition = transform.position;

        if (EnemyManager.Instance.IsDefeated(gameObject))
        {
            IsDefeated = true; // Ensure this matches your logic for determining if an enemy is defeated
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < followDistance)
        {
            if (!hasStartedFollowing)
            {
                StartFollowingPlayer();
                animator.ResetTrigger("IsDissapearing");
                animator.SetTrigger("IsAppearing");
            }
        }
        else if (distanceToPlayer > followDistance && hasStartedFollowing)
        {
            StopFollowingPlayer();
        }
    }

    private void StartFollowingPlayer()
    {
        hasStartedFollowing = true;
        isFollowingTriggered = true; 

        AudioSource.Play();
        animator.SetTrigger("IsAppearing");
        animator.SetTrigger("IsIdle");
    }

    private void StopFollowingPlayer()
    {
        hasStartedFollowing = false;
        isFollowingTriggered = false;
        animator.SetTrigger("IsDissapearing");
        rigidBody.velocity = Vector2.zero;
    }

    private void FollowPlayer()
    {
        targetPosition = Vector2.Lerp(targetPosition, player.transform.position, followDelay * Time.deltaTime);

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rigidBody.velocity = direction * followSpeed;

        if (direction.x > 0 && facingLeft)
        {
            flip();
            facingLeft = false;
        }
        else if (direction.x < 0 && !facingLeft)
        {
            flip();
            facingLeft = true;
        }
    }


    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
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
                    playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
                }
                Defeat();
            }
            else
            {
                PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();
                if (playerLife != null)
                {
                    playerLife.Die();
                }
            }
        }
    }

    private void Defeat()
    {
        AudioSource.Play();
        animator.SetTrigger("IsDissapearing");
        IsDefeated = true;
        EnemyManager.Instance.RegisterDefeat(gameObject.name);
        Destroy(gameObject, 0.4f);
    }

    private void FixedUpdate()
    {
        if (isFollowingTriggered)
        {
            FollowPlayer();
        }
    }

}
