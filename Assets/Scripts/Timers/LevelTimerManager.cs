using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelTimerManager : MonoBehaviour, ICanBePaused
{
    public static LevelTimerManager Instance;

    private float levelTimer;
    private bool isRunning;
    private bool hasStarted;
    private Field<bool> isHide = new Field<bool>(true);

    public Field<bool> isHideField => isHide;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
 
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        RunManager.Instance.RunStarted += TurnOnTimer;
        RunManager.Instance.RunEnded += TurnOffTimer;
        PauseManager.OnPauseChanged += OnPausedChanged;
    }

    private void OnDisable()
    {
        PauseManager.OnPauseChanged -= OnPausedChanged;
        RunManager.Instance.RunStarted -= TurnOnTimer;
        RunManager.Instance.RunEnded -= TurnOffTimer;
    }

    private void TurnOnTimer()
    {

        if (!hasStarted)
        {
            //Debug.Log("StartTimer();");
            ShowTimer();
            StartTimer();
            hasStarted = true;
        }
    }
    private void Update()
    {
        if (isRunning)
        {
            levelTimer += Time.deltaTime;
        }
    }

    private void TurnOffTimer()
    {
        StopTimer();
        Debug.Log($"levelTimer = {levelTimer}");
        RunStatsCollector.Instance.SetSurvivedTime(levelTimer);
        HideTimer();
    }

    private void StartTimer() => isRunning = true;
    private void StopTimer() => isRunning = false;
    public void ResetTimer()
    {
        levelTimer = 0f;
        isRunning = false;
    }
    public void HideTimer() => isHide.Value = true; 
    public void ShowTimer() =>  isHide.Value = false;
    public float GetTimer => levelTimer;
    private void OnDestroy()
    {
        Debug.Log("LevelTimerManager destroyed");
    }

    public void OnPausedChanged(bool paused)
    {
        if (paused)
            StopTimer();
        else if (hasStarted)
            StartTimer();
    }

}
