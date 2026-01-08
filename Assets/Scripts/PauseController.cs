using QuantumTek.SimpleMenu;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance { get; private set; }

    [SerializeField] private SM_Window pauseWindow;
    private PlayerAction _inputActions;
    private static bool canPause = true;
    private void Awake()
    {
        _inputActions = InputManager.Instance.PlayerActions;
        if (Instance != this)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        _inputActions.Player.Pause.started += TogglePause;
    }
    private void OnDisable()
    {
        _inputActions.Player.Pause.started -= TogglePause;
    }
    public void SetPause(bool pause)
    {
        Debug.Log($"SetPause({pause}); canpause = {canPause}");
        if (!canPause) return;

        PauseManager.InternalSetPause(pause);
        Time.timeScale = pause ? 0.0f : 1.0f;
    }
    public void TogglePause(InputAction.CallbackContext context)
    {
        if (!canPause) return;

        bool valueToSend = !PauseManager.IsPaused;

        pauseWindow.Toggle(valueToSend);
        PauseManager.InternalSetPause(valueToSend);
        Time.timeScale = valueToSend ? 0.0f : 1.0f;
    }
    public static void SetCanPause(bool value)
    {
        canPause = value;
    }
}
public static class PauseManager
{
    public static event Action<bool> OnPauseChanged;
    private static bool isPaused = false;

    public static bool IsPaused => isPaused;

    internal static void InternalSetPause(bool pause)
    {
        Debug.Log($"pause = {pause}");
        if (isPaused == pause)
            return;

        Debug.Log($"pause = {pause}");
        isPaused = pause;
        OnPauseChanged?.Invoke(isPaused);
    }
}
