using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Progress;
[CreateAssetMenu(fileName = "New Inventory", menuName = "InventorySystem/Inventory")]
public class InventoryObject : ScriptableObject
{
    public int CountSlots = 15;
    public InventorySlot[] InventorySlots;
    public Action<BaseItemObject> OnItemEquiped, OnItemAdded, OnItemUnequiped;
    private int EquippedWeaponIndex = -1;
    private void OnEnable()
    {
        EquippedWeaponIndex = -1;
        if (InventorySlots == null || InventorySlots.Length != CountSlots || InventorySlots[0] == null)
        {
            InventorySlots = new InventorySlot[CountSlots];
            for (int i = 0; i < CountSlots; i++)
            {
                InventorySlots[i] = new InventorySlot(i, null, 0);
            }
        }
    }

    public bool SetEmptySlot(BaseItemObject itemObject, int amount)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (InventorySlots[i].Item == null)
            {
                InventorySlots[i].UpdateSlot(i, itemObject, amount);
                OnItemAdded?.Invoke(itemObject);
                return true;
            }
        }
        return false;
    }

    public void SwapItems(int i1, int i2)
    {
        //Debug.Log($"i1 - {i1}; i2 - {i2}");
        InventorySlot temp = InventorySlots[i1];
        InventorySlots[i1] = InventorySlots[i2];
        InventorySlots[i2] = temp;
    }

    public bool EquipItem(int index)
    {
        if(InventorySlots[index].Item != null)
            if (InventorySlots[index].Item.CanEquip && !InventorySlots[index].IsEquipped)
            {
                Debug.Log($"EquippedWeaponIndex - {EquippedWeaponIndex}");
                if (EquippedWeaponIndex != -1)
                {
                    UnequipItem(EquippedWeaponIndex);
                }
                InventorySlots[index].IsEquipped = true;
                EquippedWeaponIndex = index;
                OnItemEquiped?.Invoke(InventorySlots[index].Item);
                return true;
            }
        return false;
    }


    public bool DropItem(int index)
    {
        if (InventorySlots[index]?.Item )
        {
            InventorySlots[index].Item = null;
            InventorySlots[index].Amount = 0;
            return true;
        }
        return false;
    }

    public bool UnequipItem(int selectedIndex)
    {
        if (InventorySlots[selectedIndex].Item.CanUnequip)
        {
            EquippedWeaponIndex = -1;
            InventorySlots[selectedIndex].IsEquipped = false;
            OnItemUnequiped?.Invoke(InventorySlots[selectedIndex].Item);
            return true;
        }
        else return false;
    }

    public bool SlotIsEquiped(int selectedIndex)
    {
        return InventorySlots[selectedIndex].IsEquipped;
    }

    public bool IsEquipped(BaseItemObject baseItem)
    {
        foreach (var slot in InventorySlots)
        {
            Debug.Log($"{slot.Item?.DisplayName} = {baseItem.DisplayName}");
            if( slot.Item == baseItem)
            {
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
    public bool IsEquipped = false;

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
