using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideTransition : TransitionBase
{
    IStateMachine stateMachine;
    Field<bool> isGrounded, isTouchingLeftWall, isTouchingRightWall;
    Field<float> yVelosity;

    public WallSlideTransition(
        IStateMachine _stateMachine,
        Field<bool> _isGrounded,
        Field<bool> _isTouchingLeftWall,
        Field<bool> _isTouchingRightWall,
        Field<float> _yVelosity)
    {
        stateMachine = _stateMachine;
        isGrounded = _isGrounded;
        isTouchingLeftWall = _isTouchingLeftWall;
        isTouchingRightWall = _isTouchingRightWall;
        yVelosity = _yVelosity;
    }

    public override void OnEnable()
    {
        isGrounded.OnValueChanged += OnIsGroundedChanged;
        isTouchingLeftWall.OnValueChanged += OnIsTouchingLeftWallChanged;
        isTouchingRightWall.OnValueChanged += OnIsTouchingRightWallChanged;
        yVelosity.OnValueChanged += OnYVelocityChanged;
    }

    public override void OnDisable()
    {
        isGrounded.OnValueChanged -= OnIsGroundedChanged;
        isTouchingLeftWall.OnValueChanged -= OnIsTouchingLeftWallChanged;
        isTouchingRightWall.OnValueChanged -= OnIsTouchingRightWallChanged;
        yVelosity.OnValueChanged -= OnYVelocityChanged;
    }

    protected override void TryTransition()
    {
        if (isGrounded.Value && (isTouchingLeftWall.Value || isTouchingRightWall.Value) && yVelosity.Value < 0f)
        {
            stateMachine.ChangeState<PlayerWallSlideState>();
        }
    }

    private void OnIsGroundedChanged(bool grounded)
    {
        TryTransition();
    }

    private void OnIsTouchingLeftWallChanged(bool touching)
    {
        TryTransition();
    }

    private void OnIsTouchingRightWallChanged(bool touching)
    {
        TryTransition();
    }

    private void OnYVelocityChanged(float velocity)
    {
        TryTransition();
    }
}
