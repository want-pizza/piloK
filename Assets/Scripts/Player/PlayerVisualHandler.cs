using UnityEngine;

public class PlayerVisualHandler : MonoBehaviour
{
    [SerializeField] private AttackEventChannel attackChannel;

    //Calling by Animations
    private void OnAttackTriggerred() => attackChannel.OnAttackTriggered();
    private void OnSwingStartReceived(string name) => attackChannel.RaiseSwingStart(name);
    private void OnHitBoxOn(string direction) => attackChannel.RaiseHitBoxOn(direction);
    private void OnHitBoxOff() => attackChannel.RaiseHitBoxOff();

    private void OnSwingEndReceived() => attackChannel.RaiseSwingEnd();

    private void OnAttackEnded() => attackChannel.RaiseAttackEnded();
}
