using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Checkpoint.ResetCheckpoints();
        BackgroundMusic.instance.RestartMusic();
        SceneManager.LoadScene(0);
    }
}
