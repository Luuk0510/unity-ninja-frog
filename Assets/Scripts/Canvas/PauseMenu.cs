using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }


    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ToggleGodMode()
    {
        PlayerLife playerLife = FindObjectOfType<PlayerLife>();
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerLife != null && playerMovement != null)
        {
            playerLife.isInvincible = !playerLife.isInvincible;
            playerMovement.hasInfiniteJump = !playerMovement.hasInfiniteJump;
        }
    }

    public void ReturnToStartMenu()
    {
        GameManager.Instance.StartMenu();
    }

}
