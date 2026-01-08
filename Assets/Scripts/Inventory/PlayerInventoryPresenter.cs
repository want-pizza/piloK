using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class PlayerInventoryPresenter : InventoryPresenterBase, ICanBePaused
{
    [SerializeField] private PlayerContext playerContext;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerMovement pLayerMovement;

    protected override void OnEnable()
    {
        Debug.Log("PlayerInventoryPresenter OnEnable");
        base.OnEnable();
        inventory.OnItemEquiped += AddStats;
        inventory.OnItemUnequiped += RemoveStats;

        PauseManager.OnPauseChanged += OnPausedChanged;

        isOpen.Value = false;
        //displayInventory.gameObject.SetActive(isOpen);
    }
    private void AddStats(BaseItemObject itemObject)
    {
        if (itemObject is ISendModifires statData)
        {
            statData.Apply(playerStats);
        }
        if(itemObject is RuntimeItemData)
        {
            itemObject.OnEquip(playerContext);//!!!!!!equip runtime
        }
    }
    private void RemoveStats(BaseItemObject itemObject)
    {
        if (itemObject is ISendModifires statData)
        {
            statData.Remove(playerStats);
        }
        if (itemObject is RuntimeItemData)
        {
            itemObject.OnUnequip();
        }
    }
    protected override void ToggleInventory(InputAction.CallbackContext ctx)
    {
        isOpen.Value = !isOpen;
        displayInventory.gameObject.SetActive(isOpen);

        if (isOpen)
        {
            displayInventory.RefreshUI(inventory.InventorySlots);
            //InputManager.Instance.SwitchState(PlayerState.Inventory);
            //Debug.Log("Inventory opened, enabling input");
            EnableInventoryInput();
            EnableMoveItem();
            displayInventory.HighlightCell(selectedIndex);
            displayInventory.ShowInteractionMenu(GetInteractionHintsForSlot(selectedIndex));
        }
        else
        {
            //InputManager.Instance.SwitchState(PlayerState.Normal);
            //Debug.Log("Inventory closed, disabling input");
            DisableInventoryInput();
            DisableMoveItem();
            displayInventory.UnhighlightCell(selectedIndex);
            displayInventory.CleanInteractionMenu();
            displayInventory.HideInteractionMenu();
        }
    }
    public void RefreUI()
    {
        displayInventory.RefreshUI(inventory.InventorySlots);
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
        inventory.OnItemUnequiped -= RemoveStats;

        PauseManager.OnPauseChanged -= OnPausedChanged;
    }
    private void OnApplicationQuit()
    {
        inventory.InventorySlots = new InventorySlot[15];
    }

    public void OnPausedChanged(bool paused)
    {
        Debug.Log("ToggleInventory(paused);");
        ToggleInventory(paused);
    }
}
