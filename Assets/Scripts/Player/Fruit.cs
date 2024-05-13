using static GameManager;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private void Start()
    {
        if (FruitManager.CollectedFruit.Contains(gameObject.name))
        {
            GameManager.FruitManager.CollectedFruit.Add(gameObject.name);
            gameObject.SetActive(false);
        }
    }
}
