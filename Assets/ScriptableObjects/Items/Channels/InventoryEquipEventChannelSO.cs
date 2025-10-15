using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Inventory Equip Event Channel")]
public class InventoryEquipEventChannelSO : ScriptableObject
{
    public UnityAction<BaseItemObject> OnItemEquipped;

    public void RaiseEvent(BaseItemObject item)
    {
        OnItemEquipped?.Invoke(item);
    }
}
