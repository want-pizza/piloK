using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    public event Action RunStarted;
    public event Action RunEnded;

    private PlayerMovement player;

    private bool runStarted = false;

    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (runStarted)
            return;

        if(player != null && Mathf.Abs(player.FieldVelocityX.Value) > 0.01f)
        {
            runStarted = true;
            RunStarted?.Invoke();
        }
    }

    public void EndRun()
    {
        Debug.Log("Run Ended");
        RunEnded.Invoke();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelTimerManager.Instance.ResetTimer();

        player = FindObjectOfType<PlayerMovement>();
    }
}
