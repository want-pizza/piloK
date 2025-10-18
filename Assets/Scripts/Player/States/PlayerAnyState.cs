public class PlayerAnyState : PlayerState
{
    public PlayerAnyState(PlayerStateMachine machine, params TransitionBase[] transitions)
    {
        this.stateMachine = machine;
        this.transitions = transitions;
    }
}
