using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
[CreateAssetMenu(fileName = "New Inventory", menuName = "InventorySystem/Inventory")]
public class InventoryObject : ScriptableObject
{
    public InventorySlot[] InventorySlots = new InventorySlot[15];
    public Action<BaseItemObject> OnItemEquiped;

    public bool SetEmptySlot(BaseItemObject itemObject, int amount)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (InventorySlots[i].Item == null)
            {
                InventorySlots[i].UpdateSlot(i, itemObject, amount);
                OnItemEquiped?.Invoke(itemObject);
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public class InventorySlot
{
    public int Id = -1;
    public BaseItemObject Item;
    public int Amount;

    public InventorySlot()
    {
        Id = -1;
        Item = null;
        Amount = 0;
    }

    public InventorySlot(int id, BaseItemObject item, int amount)
    {
        Id = id;
        Item = item;
        Amount = amount;
    }

    public void UpdateSlot(int id, BaseItemObject item, int amount)
    {
        Id = id;
        Item = item;
        Amount = amount;
    }
    public void AddAmount(int value)
    {
        Amount += value;
    }
}
