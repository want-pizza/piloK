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
    }

    public override void OnEnter()
    {
        foreach (var transition in this.transitions) 
        {
            if(transition is DeathTrantision)
            {
                transition.OnEnable();
            }
        }
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
