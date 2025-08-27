using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelTimerManager : MonoBehaviour
{
    public static LevelTimerManager Instance;

    private float levelTimer;
    private bool isRunning;
    private bool hasStarted;
    private static Field<bool> isHide = new Field<bool>(false);

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

    private void Update()
    {

        if (!hasStarted && player != null && Mathf.Abs(player.FieldVelocityX.Value) > 0.01f)
        {
            Debug.Log("StartTimer();");
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
    public void HideTimer() { Debug.Log("HideTimer()"); isHide.Value = true; }
    public void ShowTimer() { Debug.Log($"ShowTimer(); {isHide.Value}"); isHide.Value = false;}
    public float GetTimer() => levelTimer;
    private void OnDestroy()
    {
        Debug.Log("LevelTimerManager destroyed");
    }
}
