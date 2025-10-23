using UnityEngine;

public class PlayerVisualHandler : MonoBehaviour
{
    [SerializeField] private AttackEventChannel attackChannel;

    //Calling by Animations
    private void OnSwingStartReceived(string name) => attackChannel.RaiseSwingStart(name);

    private void OnSwingEndReceived() => attackChannel.RaiseSwingEnd();

    private void OnAttackEnded() => attackChannel.RaiseAttackEnded();
}
