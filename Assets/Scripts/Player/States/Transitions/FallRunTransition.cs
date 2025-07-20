using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRunTransition : RunTransition
{
    private string animationBoolValueName;
    public FallRunTransition(IStateMachine stateMachine, Field<float> velocityXField, Field<bool> isGroundedField, Field<bool> isDashingField, string _animationBoolValueName)
                            : base(stateMachine, velocityXField, isGroundedField, isDashingField)
    {
        animationBoolValueName = _animationBoolValueName;
    }
    public override void TryTransition()
    {
        if (Mathf.Abs(velocityXField) >= 0.05 && isGroundedField && !isDashingField)
            stateMachine.ChangeState<PlayerRunState>(animationBoolValueName);
    }
}
