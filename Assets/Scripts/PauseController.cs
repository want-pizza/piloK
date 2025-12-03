using QuantumTek.SimpleMenu;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private SM_Window window;
    [SerializeField] private SM_Window statPanel;
    private PlayerAction _inputActions;
    private static bool canPause = true;
    private void Awake()
    {
        _inputActions = InputManager.Instance.PlayerActions;
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
        if (!canPause) return;

        PauseManager.InternalSetPause(pause);
    }
    public void TogglePause(InputAction.CallbackContext context)
    {
        if (!canPause) return;

        bool valueToSend = !PauseManager.IsPaused;

        window.Toggle(valueToSend);
        PauseManager.InternalSetPause(valueToSend);
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
