using UnityEngine;

public class PlayerVisualHandler : MonoBehaviour
{
    [SerializeField] private AttackEventChannel attackChannel;
    [SerializeField] private GameObject swingEffectPrefab;
    [SerializeField] private Transform swingOrigin;

    private void OnEnable()
    {
        attackChannel.OnAttackEvent += OnAttackEventReceived;
    }

    private void OnDisable()
    {
        attackChannel.OnAttackEvent -= OnAttackEventReceived;
    }

    private void OnAttackEventReceived(string eventName)
    {
        switch (eventName)
        {
            case "AttackTriggered":
                Instantiate(swingEffectPrefab, swingOrigin.position, swingOrigin.rotation);
                break;
            case "SwingStart":
                Debug.Log("Swing started � enable hitbox");
                break;
            case "SwingEnd":
                Debug.Log("Swing ended � disable hitbox");
                break;
        }
    }

    // ����������� � Animation Events
    public void OnSwingStart() => attackChannel.RaiseEvent("SwingStart");
    public void OnSwingEnd() => attackChannel.RaiseEvent("SwingEnd");
}
