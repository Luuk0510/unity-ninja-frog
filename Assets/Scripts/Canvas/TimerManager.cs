using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;
    [SerializeField] private float defaultTime = 999f;

    [NonSerialized] public float currentTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentTime = defaultTime;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ResetTimer()
    {
        currentTime = defaultTime;
    }

    public void SubtractTime(float time)
    {
        currentTime -= time;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
