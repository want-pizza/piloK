using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/AttackEventChannel")]
public class AttackEventChannel : ScriptableObject
{
    public UnityAction<int> OnAttackTriggered;

    public void RaiseEvent(int comboIndex)
    {
        OnAttackTriggered?.Invoke(comboIndex);
    }
}
