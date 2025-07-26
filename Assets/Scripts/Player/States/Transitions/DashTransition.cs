using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTransition : Transition
{
    private IStateMachine stateMachine;
    private Field<bool> isDashing;
    public DashTransition(IStateMachine stateMachine, Field<bool> isDashing)
    {
        this.stateMachine = stateMachine;
        this.isDashing = isDashing;
    }
    public override void OnDisable()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnable()
    {
        isDashing.OnValueChanged += OnIsDashingChanged;
    }

    protected override void TryTransition()
    {
        if (isDashing)
            stateMachine.ChangeState<PlayerDashState>();
    }
    private void OnIsDashingChanged(bool value)
    {
        TryTransition();
    }
}
