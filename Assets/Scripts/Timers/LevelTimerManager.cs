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

    [SerializeField] private PlayerMovement player;

    public Field<bool> isHideField => isHide;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        PauseManager.OnPauseChanged += OnPausedChanged;
    }

    private void OnDisable()
    {
        PauseManager.OnPauseChanged -= OnPausedChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void Update()
    {

        if (!hasStarted && player != null && Mathf.Abs(player.FieldVelocityX.Value) > 0.01f)
        {
            //Debug.Log("StartTimer();");
            ShowTimer();
            StartTimer();
            hasStarted = true;
        }

        if (isRunning)
        {
            levelTimer += Time.deltaTime;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetTimer();
        hasStarted = false;

        player = FindObjectOfType<PlayerMovement>();
    }

    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;
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
