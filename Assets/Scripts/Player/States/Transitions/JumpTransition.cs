using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTransition : Transition
{
    Field<bool> isGrounded, isJumping;
    Field<float> velocityY;

    public JumpTransition(IStateMachine stateMachine, Field<bool> isGrounded, Field<bool> isJumping, Field<float> velocityY)
    {
        this.isGrounded = isGrounded;
        this.isJumping = isJumping;
        this.velocityY = velocityY;
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
            stateMachine.ChangeState<PlayerJumpState>();
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
