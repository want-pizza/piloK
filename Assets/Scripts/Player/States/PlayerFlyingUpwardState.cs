using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyingUpwardState : PlayerState
{
    public PlayerFlyingUpwardState(PlayerStateMachine _stateMachine, string _animationName, params TransitionBase[] _transitions)
    {
        stateMachine = _stateMachine;
        transitions = _transitions;
        animationName = _animationName;
    }
    public override void OnEnter()
    {
        Debug.Log("FlyingUpwardState OnEnter()");
        base.OnEnter();
        SelectAnimation();
    }
}
