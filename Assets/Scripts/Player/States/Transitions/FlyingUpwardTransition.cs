using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FlyingUpwardTransition : TransitionBase
{
    Field<bool> isGrounded, isDashing;
    Field<float> velosityY;

    public FlyingUpwardTransition(IStateMachine stateMachine, Field<bool> isGrounded, Field<float> velosityY, Field<bool> isDashing)
    {
        this.stateMachine = stateMachine;
        this.isGrounded = isGrounded;
        this.velosityY = velosityY;
        this.isDashing = isDashing;
    }
    public override void OnEnable()
    {
        SubscribeToPause();
        isGrounded.OnValueChanged += OnIsGroundedChanged;
        velosityY.OnValueChanged += OnVelosityYChanged;
        isDashing.OnValueChanged += OnIsDashingChanged;
    }
    public override void OnDisable()
    {
        UnsubscribeFromPause();
        isGrounded.OnValueChanged -= OnIsGroundedChanged;
        velosityY.OnValueChanged -= OnVelosityYChanged;
        isDashing.OnValueChanged -= OnIsDashingChanged;
    }
    protected override void TryTransition()
    {
        if (!isGrounded && velosityY > 0.1f)
            stateMachine.ChangeState<PlayerFlyingUpwardState>();
    }
    private void OnIsGroundedChanged(bool isGrounded)
    {
        TryTransition();
    }
    private void OnVelosityYChanged(float veloityY)
    {
        TryTransition();
    }
    private void OnIsDashingChanged(bool obj)
    {
        TryTransition();
    }
}
