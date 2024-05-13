using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private AudioSource AudioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.Play();
        if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            GameManager.Instance.NextLevel = 0;
        }
        GameManager.Instance.LoadNextLevel();
    }
}
