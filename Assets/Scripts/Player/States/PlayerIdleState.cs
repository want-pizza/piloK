using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public override bool CanOpenInventory() => true;
    public PlayerIdleState(PlayerStateMachine _stateMachine, string _animationName, params TransitionBase[] _transitions)
    {
        stateMachine = _stateMachine;
        //Debug.Log($"count transitions - {_transitions.Length}");
        animationName = _animationName;
        transitions = _transitions;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.ChangeAnimationSpeed(1f);
        SelectAnimation();
        //Debug.Log($"WasEntered - {animationName}, transition - {transitions[0]}");
    }
}
