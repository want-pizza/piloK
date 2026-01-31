using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneBase : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        Debug.Log($"LoadScene - {scene}");
        SceneLoader.Instance.StartLoadScene(scene);
    }
}
