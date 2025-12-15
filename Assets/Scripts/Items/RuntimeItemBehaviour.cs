using UnityEngine;

public abstract class RuntimeItemBehaviour : MonoBehaviour
{
    protected PlayerContext context;

    public void Initialize(PlayerContext ctx)
    {
        context = ctx;
        OnInitialize();
    }
    public void Dispose() => OnDispose();

    protected virtual void OnInitialize() { }
    protected virtual void OnDispose() { }
}
