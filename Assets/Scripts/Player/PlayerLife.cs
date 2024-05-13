using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [NonSerialized] public bool isInvincible = false;
    [SerializeField] private AudioSource deathSoundEffect;

    private Rigidbody2D myRigidbody2D; // Naam gewijzigd om waarschuwing te voorkomen
    private Animator animator;
    private CameraController cameraController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody2D = GetComponent<Rigidbody2D>(); // Gebruik de hernoemde variabele
        cameraController = FindObjectOfType<CameraController>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }


    public void Die()
    {
        if (isInvincible) return;

        deathSoundEffect.Play();
        myRigidbody2D.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("death");
        EnemyManager.Instance.ClearRecentDefeats();
        Invoke(nameof(Respawn), 1f);
    }

    private void Respawn()
    {
        transform.position = Checkpoint.LastCheckpointPosition;

        if (cameraController != null)
        {
            cameraController.ResetCameraPosition();
        }

        myRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        animator.ResetTrigger("death");
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
