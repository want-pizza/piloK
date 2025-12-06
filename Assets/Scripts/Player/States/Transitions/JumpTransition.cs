using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTransition : TransitionBase
{
    Field<bool> isJumping;

    private string animationTransition;
    public JumpTransition(IStateMachine stateMachine, Field<bool> isJumping, string transitionAnimationName)
    {
        this.stateMachine = stateMachine;
        this.isJumping = isJumping;
        this.animationTransition = transitionAnimationName;
    }
    public override void OnEnable()
    {
        SubscribeToPause();
        isJumping.OnValueChanged += OnIsJumpingChanged;
    }
    public override void OnDisable()
    {
        UnsubscribeFromPause();
        isJumping.OnValueChanged -= OnIsJumpingChanged;
    }

    protected override void TryTransition()
    {
        DebugFields();
        if (isJumping)
            stateMachine.ChangeState<PlayerFlyingUpwardState>(animationTransition);
    }
    private void OnIsJumpingChanged(bool isJumping)
    {
        TryTransition();
    }
    protected override void DebugFields()
    {
        //Debug.Log($"JumpTransition - isJumping = {isJumping};");
    }
}
