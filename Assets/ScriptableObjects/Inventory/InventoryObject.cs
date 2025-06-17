using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory", menuName = "InventorySystem/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> InventorySlots = new List<InventorySlot>();
    public Action<BaseItemObject> OnItemEquiped;
    public void AddItem(BaseItemObject itemObject, int amount)
    {
        Debug.Log($"start");
        bool hasItem = false;
        if (itemObject.IsStackable)
        {
            foreach (InventorySlot slot in InventorySlots)
            {
                if (slot.Item == itemObject)
                {
                    Debug.Log($"Item was in inventory");
                    slot.AddAmount(amount);
                    hasItem = true;
                    break;
                }
            }
        }
        else if (!hasItem)
        {
            InventorySlots.Add(new InventorySlot(itemObject, amount));
            OnItemEquiped.Invoke(itemObject);
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public BaseItemObject Item;
    public int Amount;

    public InventorySlot(BaseItemObject item, int amount)
    {
        Item = item;
        Amount = amount;
    }
    public void AddAmount(int value)
    {
        Amount += value;
    }
}
