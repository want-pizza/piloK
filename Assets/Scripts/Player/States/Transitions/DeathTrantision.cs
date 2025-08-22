using UnityEngine;

public class DeathTrantision : TransitionBase
{
    IStateMachine stateMachine;
    private Field<bool> isDead;

    public DeathTrantision(IStateMachine stateMachine, Field<bool> isDead)
    {
        this.stateMachine = stateMachine;
        this.isDead = isDead;
    }

    public override void OnEnable()
    {
        isDead.OnValueChanged += OnIsDeadChanged;
    }
    public override void OnDisable()
    {
        isDead.OnValueChanged -= OnIsDeadChanged;
    }

    protected override void TryTransition()
    {
        if (isDead == true)
            stateMachine.ChangeState<PlayerDeathState>();
    }
    private void OnIsDeadChanged(bool value) 
    {
        TryTransition();
    }
}
