using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Attack Event Channel")]
public class AttackEventChannel : ScriptableObject
{
    public UnityAction<string> OnAttackEvent;

    public void RaiseEvent(string eventName)
    {
        OnAttackEvent?.Invoke(eventName);
    }
}
