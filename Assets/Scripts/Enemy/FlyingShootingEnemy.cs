using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingShootingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float followSpeed = 2.5f;
    [SerializeField] private float followDistance = 10f;
    [SerializeField] private float shootingInterval = 2f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private AudioSource shootingAudioSource;
    [SerializeField] private AudioSource hitAudioSource;

    private float shootingTimer;
    private bool facingLeft = false;
    public bool IsDefeated { get; private set; } = false;
    private Vector2 spawnOffset = new Vector2(1.2f, 0.0f);
    private GameObject player;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 targetPosition;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        shootingTimer = shootingInterval;

        if (EnemyManager.Instance.IsDefeated(gameObject))
        {
            IsDefeated = true; // Ensure this matches your logic for determining if an enemy is defeated
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        FollowPlayer();
        HandleShooting();
    }

    private void FollowPlayer()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < followDistance)
        {
            targetPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);

            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

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
    }

    private void HandleShooting()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= followDistance)
        {
            shootingTimer -= Time.deltaTime;
            if (shootingTimer <= 0)
            {
                shootingTimer = shootingInterval;
                ShootBullet();
            }
        }
    }

    private void ShootBullet()
    {
        if (bulletPrefab != null)
        {
            shootingAudioSource.Play();

            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

            Vector2 bulletSpawnPosition = (Vector2)transform.position + Vector2.Scale(directionToPlayer, spawnOffset);

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = directionToPlayer * 5f; 
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 hitPosition = collision.contacts[0].normal;
            if (hitPosition.y <= -0.5) 
            {
                animator.SetTrigger("IsHit");

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
        GameObject audioPlayer = new GameObject("TempAudio");
        AudioSource newAudioSource = audioPlayer.AddComponent<AudioSource>();

        newAudioSource.clip = hitAudioSource.clip;
        newAudioSource.volume = hitAudioSource.volume;
        newAudioSource.pitch = hitAudioSource.pitch;
        newAudioSource.Play();

        Destroy(audioPlayer, hitAudioSource.clip.length);
        animator.SetTrigger("IsKilled");
        IsDefeated = true;
        EnemyManager.Instance.RegisterDefeat(gameObject.name);
        Destroy(gameObject, 0.4f);
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

}
