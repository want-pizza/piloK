using System;
using System.Collections.Generic;
using UnityEngine.Video;
public abstract class Transition
{
    protected IStateMachine stateMachine;
    public abstract void OnEnable();
    public abstract void OnDisable();
    protected abstract void TryTransition();
    protected virtual void DebugFields() { }
}
