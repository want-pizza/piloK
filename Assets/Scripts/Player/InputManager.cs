using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public PlayerState CurrentState { get; private set; }
    public PlayerAction PlayerActions { get; private set; }

    public void SwitchState(PlayerState newState)
    {
        //if(...){...} else ...
        CurrentState = newState;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayerActions = new PlayerAction();
        PlayerActions.Enable();
    }
}
