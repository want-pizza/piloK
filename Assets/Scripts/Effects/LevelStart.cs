using UnityEngine;

public class LevelStart : MonoBehaviour
{
    [SerializeField] private IrisListener irisListener;

    private void Start()
    {
        if (irisListener != null)
        {
            irisListener.PlayIris();
        }
        PauseController.SetCanPause(true);
    }
}
