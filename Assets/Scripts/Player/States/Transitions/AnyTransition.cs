using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyTransition : TransitionBase
{
    public AnyTransition(IStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void OnEnable()
    {
        TryTransition();
    }
    public override void OnDisable()
    {
        
    }
    protected override void TryTransition()
    {
        stateMachine.ChangeState<PlayerAnyState>();
    }
}
