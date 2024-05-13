using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyGroundFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed = 1.5f;
    [SerializeField] private float followDistance = 7;
    [SerializeField] private float idleDistance = 10f; // Add this for when the enemy should idle
    [SerializeField] private float followDelay = 1f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private AudioSource attackAudioSource;

    public bool IsDefeated { get; private set; } = false;
    private bool hasStartedFollowing = false;
    private bool isFollowingTriggered = false;
    private bool facingLeft = true;

    private Vector2 targetPosition;
    private GameObject player;
    private Rigidbody2D rigidBody;
    private Animator animator;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        targetPosition = new Vector2(transform.position.x, rigidBody.position.y);

        if (EnemyManager.Instance.IsDefeated(gameObject))
        {
            IsDefeated = true; // Ensure this matches your logic for determining if an enemy is defeated
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < followDistance && !hasStartedFollowing)
        {
            StartFollowingPlayer();
        }
        else if (distanceToPlayer > idleDistance)
        {
            StopFollowingPlayer();
        }
    }

    private void StartFollowingPlayer()
    {
        hasStartedFollowing = true;
        isFollowingTriggered = true;
        animator.ResetTrigger("IsIdle");
        animator.SetTrigger("IsFollowing");
    }

    private void StopFollowingPlayer()
    {
        hasStartedFollowing = false;
        isFollowingTriggered = false;
        animator.ResetTrigger("IsFollowing");
        animator.SetTrigger("IsIdle");
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
    }

    private void FollowPlayer()
    {
        // Update the target position with the player's x position and this object's constant y position
        targetPosition = new Vector2(player.transform.position.x, rigidBody.position.y);
        Vector2 direction = (targetPosition - new Vector2(transform.position.x, rigidBody.position.y)).normalized;

        rigidBody.velocity = new Vector2(direction.x * followSpeed, rigidBody.velocity.y);

        if (direction.x > 0 && facingLeft || direction.x < 0 && !facingLeft)
        {
            flip();
        }
    }

    private void flip()
    {
        facingLeft = !facingLeft;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void FixedUpdate()
    {
        if (isFollowingTriggered)
        {
            FollowPlayer();
        }
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
        attackAudioSource.Play();
        animator.ResetTrigger("IsFollowing");
        animator.SetTrigger("IsHit");
        IsDefeated = true;
        EnemyManager.Instance.RegisterDefeat(gameObject.name);
        Destroy(gameObject, 0.3f);
    }


}
