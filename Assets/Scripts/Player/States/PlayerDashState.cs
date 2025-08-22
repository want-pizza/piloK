using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerStateMachine stateMachine, string animationName, params TransitionBase[] transitions)
    {
        this.stateMachine = stateMachine;
        this.animationName = animationName;
        this.transitions = transitions;
    }
    public override void OnEnter()
    {
        Debug.Log("DashState - OnEnter");
        base.OnEnter();
        SelectAnimation();
    }
}
