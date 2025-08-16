using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTransition : Transition
{
    protected IStateMachine stateMachine;
    protected Field<float> xVelocityField;
    protected Field<bool> isGroundedField, isInventoryOpenField, isDashing, isDead;
    public IdleTransition(IStateMachine _stateMachine, Field<float> _xVelocityField, Field<bool> _isGroundedField, Field<bool> _isInventoryOpenField, Field<bool> isDashing, Field<bool> isDead) 
    {
        stateMachine = _stateMachine;
        xVelocityField = _xVelocityField;
        isGroundedField = _isGroundedField;
        isInventoryOpenField = _isInventoryOpenField;
        this.isDashing = isDashing;
        this.isDead = isDead;
    }
    public override void OnEnable()
    {
        xVelocityField.OnValueChanged += OnXVelosityChanged;
        isGroundedField.OnValueChanged += OnIsGroundedChanged;
        isInventoryOpenField.OnValueChanged += OnIsInventoryOpenChanged;
        isDashing.OnValueChanged += OnIsDashingChanged;
        isDead.OnValueChanged += OnIsDeadChanged;
    }
    public override void OnDisable()
    {
        xVelocityField.OnValueChanged -= OnXVelosityChanged;
        isGroundedField.OnValueChanged -= OnIsGroundedChanged;
        isInventoryOpenField.OnValueChanged -= OnIsInventoryOpenChanged;
        isDashing.OnValueChanged -= OnIsDashingChanged;
        isDead.OnValueChanged -= OnIsDeadChanged;
    }
    protected override void TryTransition()
    {
        if (xVelocityField == 0 && isGroundedField && !isInventoryOpenField && !isDead)
            stateMachine.ChangeState<PlayerIdleState>();
    }

    private void OnXVelosityChanged(float velocity)
    {
        TryTransition();
    }
    private void OnIsGroundedChanged(bool grounded)
    {
        //Debug.Log($"OnIsGroundedChanged - {grounded}");
        TryTransition();
    }
    private void OnIsInventoryOpenChanged(bool isOpen)
    {
        TryTransition();
    }
    private void OnIsDashingChanged(bool isDashing)
    {
        TryTransition();
    }
    private void OnIsDeadChanged(bool value)
    {
        TryTransition();
    }
}
