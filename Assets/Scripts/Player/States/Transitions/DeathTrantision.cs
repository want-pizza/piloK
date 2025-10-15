using UnityEngine;

public class DeathTrantision : TransitionBase
{
    IStateMachine stateMachine;
    private Field<bool> isDead;
    private bool isEnable = false;

    public DeathTrantision(IStateMachine stateMachine, Field<bool> isDead)
    {
        this.stateMachine = stateMachine;
        this.isDead = isDead;
    }

    public override void OnEnable()
    {
        isDead.OnValueChanged += OnIsDeadChanged;
        isEnable = true;
    }
    public override void OnDisable()
    {
        isDead.OnValueChanged -= OnIsDeadChanged;
        isEnable = false;
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
