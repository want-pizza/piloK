using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private InventoryObject inventoryObject;
    private void OnEnable()
    {
        inventoryObject.OnItemEquiped += RecalculateStats;
    }
    public void TryPickupItem(BaseItemObject item, int amount)
    {
        Debug.Log($"TryPickupItem {item.name}, {amount}");
        inventoryObject.AddItem(item, amount);
    }
    public void RecalculateStats(BaseItemObject itemObject)
    {
        if(itemObject is ISendModifires statData)
        {
            statData.Apply(playerStats);
        }
    }
    private void OnDisable()
    {
        inventoryObject.OnItemEquiped -= RecalculateStats;
    }
    private void OnApplicationQuit()
    {
        inventoryObject.InventorySlots.Clear();
    }
}
