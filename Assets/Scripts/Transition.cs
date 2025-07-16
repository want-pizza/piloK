using System;
using System.Collections.Generic;
public abstract class Transition
{
    protected IStateMachine stateMachine;
    public abstract void OnEnable();
    public abstract void OnDisable();
    public abstract void TryTransition();
}
