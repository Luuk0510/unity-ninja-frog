using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    [SerializeField] private int level = 1;
    [SerializeField] private AudioSource AudioSource;
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.Play();
        GameManager.Instance.NextLevel = level;
        GameManager.Instance.LoadNextLevel();
        //SceneManager.LoadScene(level);
    }

}
