using UnityEngine;
public class RunTransition : TransitionBase
{
    protected Field<float> velocityXField;
    protected Field<bool> isGroundedField;
    protected Field<bool> isDashingField;

    public RunTransition(IStateMachine stateMachine, Field<float> velocityXField, Field<bool> isGroundedField, Field<bool> isDashingField)
    {
        this.stateMachine = stateMachine;
        this.velocityXField = velocityXField;
        this.isGroundedField = isGroundedField;
        this.isDashingField = isDashingField;
    }

    public override void OnEnable()
    {
        SubscribeToPause();
        //Debug.Log("OnEnable");
        velocityXField.OnValueChanged += OnXVelosityChanged;
        isGroundedField.OnValueChanged += OnIsGroundedChanged;
        isDashingField.OnValueChanged += OnIsDashingChanged;
    }

    public override void OnDisable()
    {
        UnsubscribeFromPause();
        //Debug.Log("OnDisable");
        velocityXField.OnValueChanged -= OnXVelosityChanged;
        isGroundedField.OnValueChanged -= OnIsGroundedChanged;
        isDashingField.OnValueChanged -= OnIsDashingChanged;
    }

    protected override void TryTransition()
    {
        //Debug.Log($"RunTransition, TryTransition(), velocityXField ={velocityXField.Value}");
        if (Mathf.Abs(velocityXField) >= 0.05 && isGroundedField && !isDashingField)
            stateMachine.ChangeState<PlayerRunState>();
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
    private void OnIsDashingChanged(bool _isDashing)
    {
        TryTransition();
    }
}
