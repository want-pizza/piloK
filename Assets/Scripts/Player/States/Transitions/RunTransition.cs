using UnityEngine;
public class RunTransition : Transition
{
    private Field<float> velocityXField;
    private Field<bool> isGroundedField;
    private Field<bool> isDashingField;

    public RunTransition(IStateMachine stateMachine, Field<float> velocityXField, Field<bool> isGroundedField, Field<bool> isDashingField)
    {
        this.stateMachine = stateMachine;
        this.velocityXField = velocityXField;
        this.isGroundedField = isGroundedField;
        this.isDashingField = isDashingField;
    }

    public override void OnEnable()
    {
        Debug.Log("OnEnable");
        velocityXField.OnValueChanged += OnXVelosityChanged;
        isGroundedField.OnValueChanged += OnIsGroundedChanged;
        isDashingField.OnValueChanged += OnIsDashingChanged;
    }

    public override void OnDisable()
    {
        Debug.Log("OnDisable");
        velocityXField.OnValueChanged -= OnXVelosityChanged;
        isGroundedField.OnValueChanged -= OnIsGroundedChanged;
        isDashingField.OnValueChanged -= OnIsDashingChanged;
    }

    public override void TryTransition()
    {
        if (velocityXField != 0 && isGroundedField && !isDashingField)
            stateMachine.ChangeState<PlayerRunState>();
    }

    private void OnXVelosityChanged(float velocity)
    {
        TryTransition();
    }
    private void OnIsGroundedChanged(bool grounded)
    {
        Debug.Log($"OnIsGroundedChanged - {grounded}");
        TryTransition();
    }
    private void OnIsDashingChanged(bool _isDashing)
    {
        TryTransition();
    }
}
