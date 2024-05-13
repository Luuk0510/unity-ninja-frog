using System.Collections;
using UnityEngine;

public class EnemyRino : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float chargeDistance = 10f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private AudioSource attackAudioSource;

    public bool IsDefeated { get; private set; } = false;
    private bool isWaiting = false;
    private bool isCharging = false;
    private bool canCharge = true;
    private bool facingLeft = true;
    private Vector3 chargeDirection;
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();

        if (EnemyManager.Instance.IsDefeated(gameObject))
        {
            IsDefeated = true;
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (canCharge)
        {
            CheckAndCharge();
        }
    }

    private void CheckAndCharge()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (!isCharging && distanceToPlayer <= chargeDistance && Mathf.Abs(transform.position.y - player.position.y) < 0.5f)
        {
            animator.SetTrigger("IsRunning");
            isCharging = true;
            Vector3 newChargeDirection = (player.position.x > transform.position.x) ? Vector3.right : Vector3.left;

            if (newChargeDirection == Vector3.right && facingLeft)
            {
                flip();
                facingLeft = false;
            }
            else if (newChargeDirection == Vector3.left && !facingLeft)
            {
                flip();
                facingLeft = true;
            }

            chargeDirection = newChargeDirection;
        }
        else if (isCharging)
        {
            transform.position += chargeDirection * speed * Time.deltaTime;
        }
    }

    private IEnumerator WaitAndReset()
    {
        isWaiting = true;
        canCharge = false;
        yield return new WaitForSeconds(3);
        isWaiting = false;
        isCharging = false;
        canCharge = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && !isWaiting)
        {
            attackAudioSource.Play();
            animator.SetTrigger("IsHitWall");
            animator.SetTrigger("IsIdle");
            StartCoroutine(WaitAndReset());
        }
        else if (collision.gameObject.CompareTag("Trap"))
        {
            animator.SetTrigger("IsHitTop");
            Destroy(gameObject, 0.3f);
        }
        else if (collision.gameObject.CompareTag("Player"))
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
        attackAudioSource.Play();
        animator.SetTrigger("IsHitTop");
        IsDefeated = true;
        EnemyManager.Instance.RegisterDefeat(gameObject.name);
        Destroy(gameObject, 0.3f);
    }


    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
