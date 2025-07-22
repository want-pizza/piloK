using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTransition : Transition
{
    Field<bool> isGrounded;
    Field<float> velosityY;

    public FallTransition(IStateMachine stateMachine, Field<bool> isGrounded, Field<float> velosityY)
    {
        this.stateMachine = stateMachine;
        this.isGrounded = isGrounded;
        this.velosityY = velosityY;
    }
    public override void OnEnable()
    {
        isGrounded.OnValueChanged += OnIsGroundedChanged;
        velosityY.OnValueChanged += OnYVelocityChanged;
    }
    public override void OnDisable()
    {
        isGrounded.OnValueChanged -= OnIsGroundedChanged;
        velosityY.OnValueChanged -= OnYVelocityChanged;
    }
    protected override void TryTransition()
    {
        DebugFields();
        if (!isGrounded && velosityY < 0)
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
    protected override void DebugFields()
    {
        Debug.Log($"FallTransition: OnIsGroundedChanged - {isGrounded.Value}; velosityY = {velosityY.Value}");
    }
}
