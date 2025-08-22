using System;
using System.Collections.Generic;
using UnityEngine.Video;
public abstract class TransitionBase: ICanBePaused
{
    private bool isPaused = false;
    protected IStateMachine stateMachine;
    public abstract void OnEnable();
    public abstract void OnDisable();
    protected abstract void TryTransition();
    protected virtual void DebugFields() { }
    protected virtual void TryTransitionWithFilters()
    {
        if(isPaused)
            TryTransition();
    }

    public void OnPausedChanged(bool paused)
    {
        isPaused = paused;
    }

    protected void SubscribeToPause()
    {
        PauseManager.OnPauseChanged += OnPausedChanged;
    }

    protected void UnsubscribeFromPause()
    {
        PauseManager.OnPauseChanged -= OnPausedChanged;
    }
}
