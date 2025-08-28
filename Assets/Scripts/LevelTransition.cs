using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private GameObject endOfLevelMenuPrefab;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerInventoryPresenter inventoryPresenter;
    [SerializeField] private BaseItemObject baseItem;
    [SerializeField] private float time;
    [SerializeField] private string sceneName;
    [SerializeField] private float animationTime;
    [SerializeField] private bool isLeft;
    private TriggerChecker playerTriggerChecker;
    private IrisListener listener;

    private StarSelector starSelector;
    void Start()
    {
        playerTriggerChecker = transform.GetComponent<TriggerChecker>();
        playerTriggerChecker.OnTriggeredStateChanged += ShowEndOflevelMenu;
        listener = GetComponent<IrisListener>();
    }

    private void ShowEndOflevelMenu(bool value)
    {
        Debug.Log($"StartLoadScene - {value}");
        if (value)
        {
            PauseController.SetCanPause(false);
            movement.PlayLevelTransition(isLeft);
            LevelTimerManager.Instance.StopTimer();
            starSelector = Instantiate(endOfLevelMenuPrefab).GetComponent<StarSelector>();
            CheckStars();
        }
    }

    private void CheckStars()
    {
        starSelector.SetStar(0, true, "Level", Color.green);


        if (inventoryPresenter.IsItemInInventory(baseItem))
        {
            starSelector.SetStar(1, true, "Secret", Color.green);
        }
        else starSelector.SetStar(1, false, "Secret", Color.red);

        Debug.Log($"LevelTimerManager.Instance.GetTimer <= time; {LevelTimerManager.Instance.GetTimer} <= {time}");
        if (LevelTimerManager.Instance.GetTimer <= time)
        {
            starSelector.SetStar(2, true, LevelTimerUI.ConvertToString(time), Color.green);
        }
        else starSelector.SetStar(2, false, LevelTimerUI.ConvertToString(time), Color.red);
    }

    public void StartEndOfLevelIris()
    {
        LevelTimerManager.Instance.HideTimer();
        listener.PlayIris();
    }
    public void StartLoadScene()
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    public void DestroyMenu()
    {
        Destroy(starSelector.transform);
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
    private void OnDisable()
    {
        playerTriggerChecker.OnTriggeredStateChanged -= ShowEndOflevelMenu;
    }
}
