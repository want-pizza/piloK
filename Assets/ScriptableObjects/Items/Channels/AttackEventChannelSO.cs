using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Attack Event Channel")]
public class AttackEventChannel : ScriptableObject
{
    public UnityAction OnAttackTriggered;
    public UnityAction<string> OnSwingStart;
    public UnityAction<string> OnHitboxOn;
    public UnityAction OnHitboxOff;
    public UnityAction OnSwingEnd;
    public UnityAction OnAttackEnded;

    public void RaiseAttackTriggered() => OnAttackTriggered?.Invoke();
    public void RaiseSwingStart(string name) => OnSwingStart?.Invoke(name);
    public void RaiseHitBoxOn(string direction) => OnHitboxOn?.Invoke(direction);
    public void RaiseHitBoxOff() => OnHitboxOff?.Invoke();
    public void RaiseSwingEnd() => OnSwingEnd?.Invoke();
    public void RaiseAttackEnded() => OnAttackEnded?.Invoke();
}
