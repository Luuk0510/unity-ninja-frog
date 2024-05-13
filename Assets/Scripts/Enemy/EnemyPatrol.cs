using System;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    [SerializeField] private AudioSource collectionAudioSource;

    public bool IsDefeated { get; private set; } = false;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private Transform currentPoint;


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = pointB.transform;
        animator.SetBool("isRunning", true);

        if (EnemyManager.Instance.IsDefeated(gameObject))
        {
            IsDefeated = true;
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        MoveTowardsCurrentPoint();

        float distanceToPoint = Vector2.Distance(transform.position, currentPoint.position);

        if (distanceToPoint < 0.5f)
        {
            currentPoint = currentPoint == pointA.transform ? pointB.transform : pointA.transform;
            flip();
        }
    }


    private void MoveTowardsCurrentPoint()
    {
        Vector2 direction = ((Vector2)currentPoint.position - (Vector2)transform.position).normalized;
        rigidBody.velocity = direction * speed;
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
            Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
            Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 hitPosition = collision.contacts[0].normal;
            if (hitPosition.y <= -0.5)
            {
                Defeat();

                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, 15);
                }
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
        collectionAudioSource.Play();
        animator.SetTrigger("isKilled");
        IsDefeated = true;
        EnemyManager.Instance.RegisterDefeat(gameObject.name);
        Destroy(gameObject, 0.4f);
    }


}
