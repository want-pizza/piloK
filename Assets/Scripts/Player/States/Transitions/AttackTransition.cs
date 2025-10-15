using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTransition : TransitionBase
{
    private IStateMachine stateMachine;
    private Field<bool> isAttacking;

    public AttackTransition(IStateMachine stateMachine, Field<bool> isAttacking)
    {
        this.stateMachine = stateMachine;
        this.isAttacking = isAttacking;
    }

    public override void OnEnable()
    {
        isAttacking.OnValueChanged += OnIsAttackingChanged;
    }
    public override void OnDisable()
    {
        isAttacking.OnValueChanged -= OnIsAttackingChanged;
    }

    protected override void TryTransition()
    {
        stateMachine.ChangeState<PlayerAttackState>();
    }

    private void OnIsAttackingChanged(bool value)
    {
        TryTransition();
    }
    protected override void DebugFields()
    {
        Debug.Log($"isAttacking = {isAttacking.Value}");
    }
}
