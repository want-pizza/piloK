using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public override bool CanMove() => false;
    public PlayerDeathState(PlayerStateMachine stateMachine, string animationName, params Transition[] transitions)
    {
        this.stateMachine = stateMachine;
        this.transitions = transitions;
        this.animationName = animationName;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.PlayAnimation(animationName);
    }
}
