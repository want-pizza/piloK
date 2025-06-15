using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private List<Timer> activeTimers = new List<Timer>();

    public static TimerManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        for (int i = activeTimers.Count - 1; i >= 0; i--)
        {
            if (activeTimers[i].UpdateTimer(deltaTime))
            {
                activeTimers.RemoveAt(i);
            }
        }
    }

    public void AddTimer(float duration, string eventName)
    {
        activeTimers.Add(new Timer(duration, eventName));
    }
}
