using System;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour
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


    private GameObject endOflevelMenu;
    void Start()
    {
        playerTriggerChecker = transform.GetComponent<TriggerChecker>();
        playerTriggerChecker.OnTriggeredStateChanged += InstantiateEndOflevelMenu;
    }

    private void InstantiateEndOflevelMenu(bool value)
    {
        Debug.Log($"StartLoadScene - {value}");
        if (value)
        {
            PauseController.SetCanPause(false);
            movement.PlayLevelTransition(isLeft);
            LevelTimerManager.Instance.StopTimer();
            endOflevelMenu = Instantiate(endOfLevelMenuPrefab);
            CheckStars();
            SubscribeToEventsButton();
        }
    }

    private void SubscribeToEventsButton()
    {
        ButtonEventSelector buttonEventSelector = endOflevelMenu.GetComponent<ButtonEventSelector>();

        buttonEventSelector.Subscribe("Restart", HideTimer);
        buttonEventSelector.Subscribe("NextLevel", HideTimer);
        buttonEventSelector.Subscribe("MainMenu", HideTimer);

        buttonEventSelector.Subscribe("Restart", SceneLoader.Instance.StartLoadScene, SceneManager.GetActiveScene().name);
        buttonEventSelector.Subscribe("NextLevel", SceneLoader.Instance.StartLoadScene, sceneName);
        buttonEventSelector.Subscribe("MainMenu", SceneLoader.Instance.StartLoadScene, "MainMenu");

        buttonEventSelector.Subscribe("Restart", HideEndfLevelMenu);
        buttonEventSelector.Subscribe("NextLevel", HideEndfLevelMenu);
        buttonEventSelector.Subscribe("MainMenu", HideEndfLevelMenu);
    }

    private void CheckStars()
    {
        StarSelector starSelector = endOflevelMenu.GetComponent<StarSelector>();
        starSelector.SetStar("Level", true, "Level", Color.green);

        if (inventoryPresenter.IsItemInInventory(baseItem))
        {
            starSelector.SetStar("Secret", true, "Secret", Color.green);
        }
        else starSelector.SetStar("Secret", false, "Secret", Color.red);

        Debug.Log($"LevelTimerManager.Instance.GetTimer <= time; {LevelTimerManager.Instance.GetTimer} <= {time}");
        if (LevelTimerManager.Instance.GetTimer <= time)
        {
            starSelector.SetStar("Time", true, LevelTimerUI.ConvertToString(time), Color.green);
        }
        else starSelector.SetStar("Time", false, LevelTimerUI.ConvertToString(time), Color.red);
    }

    public void HideTimer()
    {
        LevelTimerManager.Instance.HideTimer();
    }

    public void HideEndfLevelMenu()
    {
        endOflevelMenu.SetActive(false);
    }
    
    private void OnDisable()
    {
        playerTriggerChecker.OnTriggeredStateChanged -= InstantiateEndOflevelMenu;
    }
}
