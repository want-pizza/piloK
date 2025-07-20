using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyingUpwardState : PlayerState
{
    public PlayerFlyingUpwardState(PlayerStateMachine _stateMachine, string _animationName, params Transition[] _transitions)
    {
        stateMachine = _stateMachine;
        transitions = _transitions;
        animationName = _animationName;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        SelectAnimation();
    }
}
