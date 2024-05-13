using System;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [NonSerialized] public static int FruitAtLastCheckpoint;
    [NonSerialized] public static bool CanDoubleJumpAtLastCheckpoint;
    [NonSerialized] public static bool CanWallJumpAtLastCheckpoint;
    [NonSerialized] public static HashSet<string> CollectedFruits = new HashSet<string>();
    [NonSerialized] public static HashSet<string> CollectedPowerUps = new HashSet<string>();
    [NonSerialized] public static HashSet<string> DefeatedEnemies = new HashSet<string>();
    [NonSerialized] public static Vector3 LastCheckpointPosition;
    [SerializeField] private AudioSource Audio;

    private static bool captured = false;
    private Animator animator;
    private static HashSet<string> activatedCheckpoints = new HashSet<string>();

    public static void ResetCheckpoints()
    {
        captured = false;
        LastCheckpointPosition = Vector3.zero;
        FruitAtLastCheckpoint = 0;
        CollectedFruits.Clear();
        CollectedPowerUps.Clear();
        DefeatedEnemies.Clear();
        if(EnemyManager.Instance != null)
        {
            EnemyManager.Instance.DefeatedEnemies.Clear();
            //EnemyManager.Instance.RecentlyDefeatedEnemies.Clear();
        }

        activatedCheckpoints.Clear();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (activatedCheckpoints.Contains(gameObject.name))
        {
            animator.SetTrigger("IsCaptured");
            DisableGameObject();
        }
    }

    private void DisableGameObject()
    {
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
        foreach (GameObject fruit in fruits)
        {
            if (CollectedFruits.Contains(fruit.name))
            {
                fruit.SetActive(false);
            }
        }

        GameObject[] doubleJumpPowerUps = GameObject.FindGameObjectsWithTag("DoubleJump");
        GameObject[] wallJumpPowerUps = GameObject.FindGameObjectsWithTag("WallJump");
        GameObject[] allPowerUps = new GameObject[doubleJumpPowerUps.Length + wallJumpPowerUps.Length];
        doubleJumpPowerUps.CopyTo(allPowerUps, 0);
        wallJumpPowerUps.CopyTo(allPowerUps, 0);

        foreach (GameObject powerUp in allPowerUps)
        {
            string powerUpIdentifier = (powerUp.tag == "DoubleJump" ? "DoubleJump:" : "WallJump:") + powerUp.name;

            if (CollectedPowerUps.Contains(powerUpIdentifier))
            {
                powerUp.SetActive(false);
            }
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (DefeatedEnemies.Contains(enemy.name))
            {
                enemy.SetActive(false);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !activatedCheckpoints.Contains(gameObject.name))
        {
            activatedCheckpoints.Add(gameObject.name);
            animator.SetTrigger("Capture");

            if (!captured)
            {
                Audio.Play();
                captured = true;
            }

            animator.SetTrigger("IsCaptured");
            
            LastCheckpointPosition = transform.position;
            CanDoubleJumpAtLastCheckpoint = collision.GetComponent<PlayerMovement>().IsAbleToDoubleJump;
            CanWallJumpAtLastCheckpoint = collision.GetComponent<PlayerMovement>().isAbleToWallJump;
            
            FruitAtLastCheckpoint = ItemCollector.FruitsCollected;
            EnemyManager.Instance.ApplyDefeatedStatus();
            ItemCollector.Instance.ResetCollectedItems();

            DisableGameObject();
        }
    }

}
