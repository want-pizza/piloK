using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    private IrisListener listener;

    private string sceneName;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        listener = GetComponent<IrisListener>();
    }

    public void StartLoadScene(string sceneName)
    {
        this.sceneName = sceneName;
        listener.Target = FindObjectOfType<PlayerMovement>().transform;
        listener.PlayIris();
    }

    public void StartLoadScene(string sceneName, Transform irisTarget)
    {
        this.sceneName = sceneName;
        listener.Target = irisTarget;
        listener.PlayIris();
    }

    public void LoadScene()
    {
        if (sceneName != null)
            StartCoroutine(LoadSceneAsync(sceneName));
        else Debug.Log("sceneName = null");
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
