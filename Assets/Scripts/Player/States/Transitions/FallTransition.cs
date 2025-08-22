using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTransition : TransitionBase
{
    Field<bool> isGrounded, isDashing;
    Field<float> velosityY;

    public FallTransition(IStateMachine stateMachine, Field<bool> isGrounded, Field<bool> isDashing, Field<float> velosityY)
    {
        this.stateMachine = stateMachine;
        this.isGrounded = isGrounded;
        this.isDashing = isDashing;
        this.velosityY = velosityY;
    }
    public override void OnEnable()
    {
        SubscribeToPause();
        isGrounded.OnValueChanged += OnIsGroundedChanged;
        velosityY.OnValueChanged += OnYVelocityChanged;
        isDashing.OnValueChanged += OnIsDashingChanged;
    }
    public override void OnDisable()
    {
        UnsubscribeFromPause();
        isGrounded.OnValueChanged -= OnIsGroundedChanged;
        velosityY.OnValueChanged -= OnYVelocityChanged;
        isDashing.OnValueChanged -= OnIsDashingChanged;
    }
    protected override void TryTransition()
    {
        DebugFields();
        if (!isGrounded && !isDashing && velosityY < 0)
            stateMachine.ChangeState<PlayerFallState>();
    }
    private void OnIsGroundedChanged(bool grounded)
    {
        //Debug.Log($"OnIsGroundedChanged - {grounded}");
        TryTransition();
    }
    private void OnYVelocityChanged(float velocity)
    {
        TryTransition();
    }
    private void OnIsDashingChanged(bool isDashing)
    {
        TryTransition();
    }
    protected override void DebugFields()
    {
        //Debug.Log($"FallTransition: OnIsGroundedChanged - {isGrounded.Value}; velosityY = {velosityY.Value}");
    }
}
