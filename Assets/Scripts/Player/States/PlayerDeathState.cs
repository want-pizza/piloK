using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public override bool CanMove() => false;
    public PlayerDeathState(PlayerStateMachine stateMachine, string animationName, params TransitionBase[] transitions)
    {
        this.stateMachine = stateMachine;
        this.transitions = transitions;
        this.animationName = animationName;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.PlayAnimation(animationName);
        PauseController.SetCanPause(false);
    }
    public override void OnExit()
    {
        base.OnExit();
        PauseController.SetCanPause(true);
    }
}
