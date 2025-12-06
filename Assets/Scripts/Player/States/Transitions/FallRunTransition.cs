using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRunTransition : RunTransition
{
    private Field<bool> isJumping;
    private string animationBoolValueName;
    public FallRunTransition(IStateMachine stateMachine, Field<float> velocityXField, Field<bool> isGroundedField, Field<bool> isDashingField, Field<bool> _isJumping, string _animationBoolValueName)
                            : base(stateMachine, velocityXField, isGroundedField, isDashingField)
    {
        isJumping = _isJumping;
        animationBoolValueName = _animationBoolValueName;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        isJumping.OnValueChanged += IsJumping_OnValueChanged;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        isJumping.OnValueChanged -= IsJumping_OnValueChanged;
    }

    private void IsJumping_OnValueChanged(bool obj)
    {
        TryTransition();
    }

    protected override void TryTransition()
    {
        //DebugFields();
        if (Mathf.Abs(velocityXField) >= 0.05 && isGroundedField && !isDashingField && !isJumping)
            stateMachine.ChangeState<PlayerRunState>(animationBoolValueName);
    }
    protected override void DebugFields()
    {
        Debug.Log($"FallRunTransition: xVelocityField - {velocityXField.Value}; isGroundedField - {isGroundedField.Value}");
    }
}
