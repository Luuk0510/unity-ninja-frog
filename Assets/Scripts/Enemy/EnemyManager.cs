using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public HashSet<string> DefeatedEnemies = new HashSet<string>();
    public HashSet<string> RecentlyDefeatedEnemies = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void RegisterDefeat(string enemyName)
    {
        RecentlyDefeatedEnemies.Add(enemyName);
    }

    public void ApplyDefeatedStatus()
    {
        foreach (string enemyName in RecentlyDefeatedEnemies)
        {
            DefeatedEnemies.Add(enemyName);
            GameObject enemy = GameObject.Find(enemyName);
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
        }
        RecentlyDefeatedEnemies.Clear();
    }

    public void ClearRecentDefeats()
    {
        RecentlyDefeatedEnemies.Clear();
    }

    public bool IsDefeated(GameObject enemy)
    {
        return DefeatedEnemies.Contains(enemy.name);
    }

    public void ResetDefeatedEnemies()
    {
        DefeatedEnemies.Clear();
    }
}
