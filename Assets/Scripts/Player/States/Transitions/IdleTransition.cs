using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTransition : Transition
{
    protected IStateMachine stateMachine;
    protected Field<float> xVelocityField;
    protected Field<bool> isGroundedField, isInventoryOpenField;
    public IdleTransition(IStateMachine _stateMachine, Field<float> _xVelocityField, Field<bool> _isGroundedField, Field<bool> _isInventoryOpenField) 
    {
        stateMachine = _stateMachine;
        xVelocityField = _xVelocityField;
        isGroundedField = _isGroundedField;
        isInventoryOpenField = _isInventoryOpenField;
    }
    public override void OnEnable()
    {
        xVelocityField.OnValueChanged += OnXVelosityChanged;
        isGroundedField.OnValueChanged += OnIsGroundedChanged;
        isInventoryOpenField.OnValueChanged += OnIsInventoryOpenChanged;
    }
    public override void OnDisable()
    {
        xVelocityField.OnValueChanged -= OnXVelosityChanged;
        isGroundedField.OnValueChanged -= OnIsGroundedChanged;
        isInventoryOpenField.OnValueChanged -= OnIsInventoryOpenChanged;
    }
    public override void TryTransition()
    {
        if (xVelocityField == 0 && isGroundedField && !isInventoryOpenField)
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
}
