using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private Text fruitText;
    [SerializeField] private AudioSource collectionAudioSource;

    public static int FruitsCollected { get; private set; } = 0; 
    [NonSerialized] public static HashSet<string> RecentlyCollectedFruits = new HashSet<string>();

    public static ItemCollector Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FruitsCollected = Checkpoint.FruitAtLastCheckpoint; 
        UpdateFruitText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
            collectionAudioSource.Play();
            Animator itemAnimator = collision.gameObject.GetComponent<Animator>();
            if (itemAnimator != null)
            {
                itemAnimator.SetTrigger("collect");
            }
            FruitsCollected++;
            UpdateFruitText();
            RecentlyCollectedFruits.Add(collision.gameObject.name);
            Destroy(collision.gameObject, 0.4f);
        }
    }

    private void UpdateFruitText()
    {
        fruitText.text = "Fruit: " + FruitsCollected;
    }

    public void ResetCollectedItems()
    {
        foreach (string fruit in RecentlyCollectedFruits)
        {
            Checkpoint.CollectedFruits.Add(fruit);
        }
        RecentlyCollectedFruits.Clear();
        // Update de UI of andere relevante systemen hier.
        //UpdateFruitText();
    }

}
