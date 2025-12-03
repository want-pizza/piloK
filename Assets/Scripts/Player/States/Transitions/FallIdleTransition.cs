using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallIdleTransition : IdleTransition
{
    private string boolValueName;
    public FallIdleTransition(IStateMachine _stateMachine, Field<float> _xVelocityField, Field<bool> _isGroundedField, Field<bool> isDashing, Field<bool> isDead, string _boolValueName)
                              : base(_stateMachine, _xVelocityField, _isGroundedField,  isDashing, isDead)
    {
        boolValueName = _boolValueName;
    }
    protected override void TryTransition()
    {
        DebugFields();
        if (xVelocityField == 0 && isGroundedField)
            stateMachine.ChangeState<PlayerIdleState>(boolValueName);
    }
    protected override void DebugFields()
    {
        //Debug.Log($"FallIdleTransition: xVelocityField - {xVelocityField.Value}; isGroundedField - {isGroundedField.Value}");
    }
}
