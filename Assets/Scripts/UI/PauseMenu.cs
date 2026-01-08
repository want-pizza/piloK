using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private static PauseMenu instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
