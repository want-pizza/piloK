using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExitTransition : TransitionBase
{
    private Field<bool> isAttacking;

    public AttackExitTransition(IStateMachine stateMachine, Field<bool> isAttacking)
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
        if (!isAttacking.Value)
            stateMachine.ChangeState<PlayerAnyState>();
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
