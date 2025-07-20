using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTransition : Transition
{
    Field<bool> isGrounded, isJumping;
    Field<float> velocityY;
    private string boolValueName;
    public JumpTransition(IStateMachine stateMachine, Field<bool> isGrounded, Field<bool> isJumping, Field<float> velocityY, string transitionAnimationName)
    {
        this.stateMachine = stateMachine;
        this.isGrounded = isGrounded;
        this.isJumping = isJumping;
        this.velocityY = velocityY;
        this.boolValueName = transitionAnimationName;
    }
    public override void OnEnable()
    {
        isGrounded.OnValueChanged += OnIsGroundedChanged;
        isJumping.OnValueChanged += OnIsJumpingChanged;
        velocityY.OnValueChanged += OnYVelocityChanged;
    }
    public override void OnDisable()
    {
        isGrounded.OnValueChanged -= OnIsGroundedChanged;
        isJumping.OnValueChanged -= OnIsJumpingChanged;
        velocityY.OnValueChanged -= OnYVelocityChanged;
    }

    public override void TryTransition()
    {
        if (!isGrounded && isJumping && velocityY > 0f)
            stateMachine.ChangeState<PlayerFlyingUpwardState>(boolValueName);
    }
    private void OnIsGroundedChanged(bool grounded)
    {
        //Debug.Log($"OnIsGroundedChanged - {grounded}");
        TryTransition();
    }
    private void OnIsJumpingChanged(bool isJumping)
    {
        TryTransition();
    }
    private void OnYVelocityChanged(float velocity)
    {
        TryTransition();
    }
}
