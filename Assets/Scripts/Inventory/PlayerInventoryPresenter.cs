using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class PlayerInventoryPresenter : InventoryPresenterBase
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private InventoryEquipEventChannelSO equipEventChannel;
    [SerializeField] private InventoryEquipEventChannelSO unequipEventChannel;

    protected override void OnEnable()
    {
        Debug.Log("PlayerInventoryPresenter OnEnable");
        base.OnEnable();
        inventory.OnItemEquiped += AddStats;
        //inventory.OnItemUnequiped += RemoveStats;
        inventory.OnItemUnequiped += UnequipItem;
        isOpen = false;
        displayInventory.gameObject.SetActive(isOpen);
    }

    private void UnequipItem(BaseItemObject @object)
    {
        Debug.Log("UnequipItem");
        unequipEventChannel.OnItemEquipped.Invoke(@object);
        //must edit UI
    }
    public void AddStats(BaseItemObject itemObject)
    {
        if(itemObject is ISendModifires statData)
        {
            statData.Apply(playerStats);
        }
    }
    private void RemoveStats(BaseItemObject @object)
    {
        throw new NotImplementedException();
    }
    protected override void ToggleInventory(InputAction.CallbackContext ctx)
    {
        isOpen = !isOpen;
        displayInventory.gameObject.SetActive(isOpen);

        if (isOpen)
        {
            displayInventory.RefreshUI(inventory.InventorySlots);
            InputManager.Instance.SwitchState(PlayerState.Inventory);
            //Debug.Log("Inventory opened, enabling input");
            EnableInventoryInput();
            EnableMoveItem();
            displayInventory.HighlightCell(selectedIndex);
            displayInventory.ShowInteractionMenu(GetInteractionHintsForSlot(selectedIndex));
        }
        else
        {
            InputManager.Instance.SwitchState(PlayerState.Normal);
            //Debug.Log("Inventory closed, disabling input");
            DisableInventoryInput();
            DisableMoveItem();
            displayInventory.UnhighlightCell(selectedIndex);
            displayInventory.CleanInteractionMenu();
            displayInventory.HideInteractionMenu();
        }
    }
    protected override void TryEquip(InputAction.CallbackContext ctx)
    {
        if (inventory.SlotIsEquiped(selectedIndex))
        {
            if (inventory.UnequipItem(selectedIndex))
            {
                RefreshInteractionMenu();
            }
            return;
        }
       
        if (!inventory.EquipItem(selectedIndex))
        {
            Debug.Log("item wasnt equiped");
        }
        else
        {
            RefreshInteractionMenu();
            BaseItemObject weapon = inventory.InventorySlots[selectedIndex].Item;

            if (weapon is WeaponItemObject)
                equipEventChannel.RaiseEvent(weapon);
        }
    }
    protected void RefreshInteractionMenu()
    {
        displayInventory.CleanInteractionMenu();
        displayInventory.ShowInteractionMenu(GetInteractionHintsForSlot(selectedIndex));
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        inventory.OnItemEquiped -= AddStats;
        inventory.OnItemUnequiped -= AddStats;
        inventory.OnItemUnequiped -= UnequipItem;
    }
    private void OnApplicationQuit()
    {
        inventory.InventorySlots = new InventorySlot[15];
    }
}
