using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState 
{
    public PlayerWallSlideState(PlayerStateMachine _stateMachine, string _animationName, params Transition[] _transitions)
    {
        stateMachine = _stateMachine;
        animationName = _animationName;
        transitions = _transitions;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.PlayAnimation(animationName);
    }
}
