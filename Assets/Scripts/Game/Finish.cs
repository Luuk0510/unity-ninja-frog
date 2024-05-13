using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] private int level = 1;
    [SerializeField] private AudioSource AudioSource;

    private int summaryLevel = 4;
    private bool captured = true;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("IsFinished");

            if (captured)
            {
                AudioSource.Play();
                captured = false;
            }

            StartCoroutine(WaitAndLoadScene());
        }
    }

    IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(3); 
        int currentFruits = ItemCollector.FruitsCollected;
        float remainingTime = TimerManager.instance.currentTime;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        GameManager.Instance.UpdateScoreAndTimeAndLevel(currentFruits, remainingTime, currentSceneIndex, level);

        SceneManager.LoadScene(summaryLevel);
    }


}
