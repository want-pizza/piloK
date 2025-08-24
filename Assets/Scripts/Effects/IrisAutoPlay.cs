using UnityEngine;

public class IrisAutoPlay : MonoBehaviour
{
    [SerializeField] private IrisListener irisListener;

    private void Start()
    {
        if (irisListener != null)
        {
            irisListener.PlayIris();
        }
    }
}
