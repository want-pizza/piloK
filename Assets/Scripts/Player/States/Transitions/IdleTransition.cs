using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTransition : Transition
{
    IStateMachine stateMachine;
    private Field<float> xVelocityField;
    Field<bool> isGroundedField, isInventoryOpenField;
    public IdleTransition(IStateMachine _stateMachine, Field<float> _xVelocityField, Field<bool> _isGroundedField, Field<bool> _isInventoryOpenField) 
    {
        stateMachine = _stateMachine;
        xVelocityField = _xVelocityField;
        isGroundedField = _isGroundedField;
        isInventoryOpenField = _isInventoryOpenField;
    }
    public override void OnEnable()
    {
        
    }
    public override void OnDisable()
    {
        throw new System.NotImplementedException();
    }
    public override void TryTransition()
    {
        throw new System.NotImplementedException();
    }


}
