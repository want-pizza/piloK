using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private string sceneName;
    [SerializeField] private float animationTime;
    [SerializeField] private bool isLeft;
    private TriggerChecker playerTriggerChecker;
    private IrisListener listener;
    void Start()
    {
        playerTriggerChecker = transform.GetComponent<TriggerChecker>();
        playerTriggerChecker.OnTriggeredStateChanged += StartTransitionAnimation;
        listener = GetComponent<IrisListener>();
    }

    private void StartTransitionAnimation(bool value)
    {
        Debug.Log($"StartLoadScene - {value}");
        if (value)
        {
            LevelTimerManager.Instance.StopTimer();
            LevelTimerManager.Instance.HideTimer();
            movement.PlayLevelTransition(isLeft);
            listener.PlayIris();
        }
    }
    public void StartLoadScene()
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            Debug.Log(operation.progress); // від 0 до 0.9
            yield return null;
        }
    }
}
