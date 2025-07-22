using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallIdleTransition : IdleTransition
{
    private string boolValueName;
    public FallIdleTransition(IStateMachine _stateMachine, Field<float> _xVelocityField, Field<bool> _isGroundedField, Field<bool> _isInventoryOpenField, string _boolValueName)
                              : base(_stateMachine, _xVelocityField, _isGroundedField, _isInventoryOpenField) 
    {
        boolValueName = _boolValueName;
    }
    protected override void TryTransition()
    {
        if (xVelocityField == 0 && isGroundedField && !isInventoryOpenField)
            stateMachine.ChangeState<PlayerIdleState>(boolValueName);
    }
}
