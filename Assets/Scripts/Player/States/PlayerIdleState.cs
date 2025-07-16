using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine _stateMachine, List<Transition> _transitions, string _animationName)
    {
        stateMachine = _stateMachine;
        transitions = _transitions;
        Debug.Log($"count transitions - {_transitions.Count}");
        animationName = _animationName;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.PlayerAnimation(animationName);
        Debug.Log($"WasEntered - {animationName}, transition - {transitions[0]}");
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
