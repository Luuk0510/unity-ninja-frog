using System.Collections;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootingInterval = 2f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private bool isShootingLeft = true;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float heightOffset = 0f;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource shootSound;

    public bool IsDefeated { get; private set; } = false;
    private float timeSinceLastShot = 0f; // Time since last shot was fired
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (!isShootingLeft) Flip();
        StartCoroutine(ShootBulletAtInterval());

        if (EnemyManager.Instance.IsDefeated(gameObject))
        {
            IsDefeated = true;
            gameObject.SetActive(false);
        }
    }

    private IEnumerator ShootBulletAtInterval()
    {
        while (true)
        {
            animator.SetTrigger("IsShooting");
            yield return new WaitForSeconds(0.4f);
            ShootBullet();
            animator.SetTrigger("IsIdle");
            yield return new WaitForSeconds(shootingInterval);

            animator.SetTrigger("IsIdle");
        }
    }

    private void ShootBullet()
    {
        if (bulletPrefab != null)
        {
            shootSound.Play();

            Vector3 horizontalOffset = isShootingLeft ? Vector3.left : Vector3.right;
            horizontalOffset *= 1f;

            Vector3 spawnPosition = transform.position + horizontalOffset + new Vector3(0, heightOffset, 0);

            GameObject newBullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            if (!isShootingLeft) newBullet.transform.localScale = new Vector3(-Mathf.Abs(newBullet.transform.localScale.x), newBullet.transform.localScale.y, newBullet.transform.localScale.z);
            else newBullet.transform.localScale = new Vector3(Mathf.Abs(newBullet.transform.localScale.x), newBullet.transform.localScale.y, newBullet.transform.localScale.z);


            Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.gravityScale = 0;
                bulletRb.velocity = (isShootingLeft ? Vector2.left : Vector2.right) * bulletSpeed;
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
        hitSound.Play();
        animator.SetTrigger("IsHit");
        IsDefeated = true;
        EnemyManager.Instance.RegisterDefeat(gameObject.name);
        Destroy(gameObject, 0.3f);
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
