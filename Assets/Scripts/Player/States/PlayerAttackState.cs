using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private PlayerStateMachine machine;
    private PlayerWeaponController combat;

    public PlayerAttackState(PlayerStateMachine machine, PlayerWeaponController combat, params TransitionBase[] transitions)
    {
        this.machine = machine;
        this.combat = combat;
        this.transitions = transitions;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        string anim = combat.GetNextAttackAnimation();
        float speed = combat.GetAttackSpeed();

        machine.PlayAnimation(anim);
        machine.ChangeAnimationSpeed(speed);

        Debug.Log($"Attack started with {anim}, speed: {speed}");
    }
    public override void OnExit() 
    {
        base.OnExit();

        machine.ChangeAnimationSpeed(1f);
    }
}
