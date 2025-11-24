using System.Collections;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private Field<string> currentAttackAnim;
    public PlayerAttackState(PlayerStateMachine machine, Field<string> currentAttackAnim, params TransitionBase[] transitions)
    {
        this.stateMachine = machine;
        this.currentAttackAnim = currentAttackAnim;
        this.transitions = transitions;
        Debug.Log($" {this.GetType()};");
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log($"animationName = {currentAttackAnim.Value}");
        animationName = currentAttackAnim.Value;
        SelectAnimation();
    }
    public override void OnExit() 
    {
        base.OnExit();

        //stateMachine.ChangeAnimationSpeed(1f);
    }
}
