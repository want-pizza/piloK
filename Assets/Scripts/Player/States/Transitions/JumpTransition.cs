using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTransition : Transition
{
    Field<bool> isJumping;

    private string boolValueName;
    public JumpTransition(IStateMachine stateMachine, Field<bool> isJumping, string transitionAnimationName)
    {
        this.stateMachine = stateMachine;
        this.isJumping = isJumping;
        this.boolValueName = transitionAnimationName;
    }
    public override void OnEnable()
    {
        isJumping.OnValueChanged += OnIsJumpingChanged;
    }
    public override void OnDisable()
    {
        isJumping.OnValueChanged -= OnIsJumpingChanged;
    }

    protected override void TryTransition()
    {
        DebugFields();
        if (isJumping)
            stateMachine.ChangeState<PlayerFlyingUpwardState>(boolValueName);
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
