using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(PlayerStateMachine _stateMachine, string _animationName, params Transition[] _transitions)
    {
        stateMachine = _stateMachine;
        transitions = _transitions;
        //Debug.Log($"count transitions - {_transitions.Length}");
        animationName = _animationName;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        SelectAnimation();
    }
}
