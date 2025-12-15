using UnityEngine;

[CreateAssetMenu(menuName = "Items/Runtime Item")]
public class RuntimeItemData : BaseItemObject
{
    public RuntimeItemBehaviour runtimePrefab;
    private RuntimeItemBehaviour instance;

    public override void OnEquip(PlayerContext context)
    {
        instance = Instantiate(runtimePrefab, context.RuntimeItemHolder);
        instance.Initialize(context);
    }

    public override void OnUnequip()
    {
        instance.Dispose();
        Destroy(instance);
    }
}
