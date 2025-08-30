using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private event Action stopDashTrail;
    public PlayerDashState(PlayerStateMachine stateMachine, string animationName, Action stopDashTrail,  params TransitionBase[] transitions)
    {
        this.stateMachine = stateMachine;
        this.animationName = animationName;
        this.transitions = transitions;
        this.stopDashTrail += stopDashTrail;
    }
    public override void OnEnter()
    {
        Debug.Log("DashState - OnEnter");
        base.OnEnter();
        SelectAnimation();
    }
    public override void OnExit()
    {
        stopDashTrail?.Invoke();
        base.OnExit();
    }
}
