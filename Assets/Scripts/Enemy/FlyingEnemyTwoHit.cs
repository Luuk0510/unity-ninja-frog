using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyTwoHit : MonoBehaviour
{
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private float followDistance = 8f;
    [SerializeField] private float followDelay = 1.5f; // De vertragingstijd in seconden
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private AudioSource attackAudioSource;

    public bool IsDefeated { get; private set; } = false;
    private bool facingLeft = true;
    private int hitCount = 0;

    private Vector2 targetPosition;
    private GameObject player;
    private Rigidbody2D rigidBody;
    private Animator animator;

    private enum State { Flying, Grounded, Defeated }
    private State currentState = State.Flying;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        targetPosition = transform.position;

        if (EnemyManager.Instance.IsDefeated(gameObject))
        {
            IsDefeated = true;
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Flying:
                FollowPlayer();
                break;
            case State.Grounded:
                FollowPlayerOnGround();
                break;
        }
    }

    private void FollowPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < followDistance)
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
            else
            {
                //rigidBody.velocity = Vector2.zero;
            }
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
                animator.SetTrigger("IsHit");

                hitCount++;

                Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRigidbody != null)
                {
                    playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
                }

                if (hitCount == 1)
                {
                    TransitionToGroundedState();
                }
                else if (hitCount >= 2)
                {
                    DefeatEnemy();
                }
            }
            else
            {
                AttackPlayer(collision);
            }
        }
    }

    private void TransitionToGroundedState()
    {
        attackAudioSource.Play();
        animator.SetTrigger("IsRunning");
        rigidBody.gravityScale = 10f;
        followSpeed = 3f;
        currentState = State.Grounded;
    }


    private void FollowPlayerOnGround()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < followDistance)
        {
            targetPosition = new Vector2(player.transform.position.x, transform.position.y);
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            rigidBody.velocity = new Vector2(direction.x * followSpeed, 0);

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
        else
        {
            rigidBody.velocity = Vector2.zero;
        }
    }


    private void DefeatEnemy()
    {
        attackAudioSource.Play();
        animator.SetTrigger("IsHit");
        rigidBody.velocity = Vector2.zero;
        rigidBody.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        IsDefeated = true;
        EnemyManager.Instance.RegisterDefeat(gameObject.name);
        Destroy(gameObject, 0.3f);
    }


    private void AttackPlayer(Collision2D collision)
    {
        attackAudioSource.Play();
        PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();
        if (playerLife != null)
        {
            playerLife.Die();
        }
    }

}
