using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine _stateMachine, string _animationName, params Transition[] _transitions)
    {
        stateMachine = _stateMachine;
        transitions = _transitions;
        Debug.Log($"count transitions - {_transitions.Length}");
        animationName = _animationName;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.PlayAnimation(animationName);
        Debug.Log($"WasEntered - {animationName}, transition - {transitions[0]}");
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
