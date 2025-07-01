using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InventoryPresenterBase : MonoBehaviour
{
    [SerializeField] protected InventoryObject inventory;
    [SerializeField] protected DisplayInventory displayInventory;
    protected bool inventoryInputEnabled;
    protected PlayerAction inputActions;
    protected bool isOpen;
    protected int selectedIndex;

    protected virtual void Awake()
    {
        inputActions = InputManager.Instance.PlayerActions;
        for (int i = 0; i < inventory.CountSlots; i++)
        {
            inventory.InventorySlots[i] = new InventorySlot(i, null, 0);
        }
        displayInventory.CreateSlots(inventory.InventorySlots);
    }

    protected virtual void OnEnable()
    {
        inventory.OnItemAdded += ShowInventoryItem;
        inputActions.Player.Inventory.started += ctx => ToggleInventory();
    }
    protected virtual void ToggleInventory()
    {
        isOpen = !isOpen;
        displayInventory.gameObject.SetActive(isOpen);

        if (isOpen)
        {
            displayInventory.RefreshUI(inventory.InventorySlots);
            EnableInventoryInput();
        }
        else
        {
            DisableInventoryInput();
        }
    }
    protected void EnableInventoryInput()
    {
        inputActions.Player.Move.performed += OnMoveInCells;
        inputActions.Player.Attack.started += OnTryEquip;
    }

    protected void DisableInventoryInput()
    {
        inputActions.Player.Move.performed -= OnMoveInCells;
        inputActions.Player.Attack.started -= OnTryEquip;
    }
    private void OnMoveInCells(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        //Debug.Log($"Inventory received move input: {input}");
        HandleInventoryNavigation(input);
    }

    private void OnTryEquip(InputAction.CallbackContext ctx)
    {
        bool itemEquiped = inventory.EquipItem(selectedIndex);
        if (!itemEquiped)
        {
            Debug.Log("item wasnt equiped");
        }
    }

    protected void HandleInventoryNavigation(Vector2 input)
    {
        int columns = displayInventory.GetNumberSlotsInColumn;
        int totalSlots = inventory.CountSlots;
        int rows = Mathf.CeilToInt((float)totalSlots / columns);

        int previousIndex = selectedIndex;

        if (input.x > 0.1f)
        {
            int col = (selectedIndex % columns + 1) % columns;
            int row = selectedIndex / columns;
            selectedIndex = row * columns + col;
        }
        else if (input.x < -0.1f)
        {
            int col = (selectedIndex % columns - 1 + columns) % columns;
            int row = selectedIndex / columns;
            selectedIndex = row * columns + col;
        }
        else if (input.y > 0.1f)
        {
            int col = selectedIndex % columns;
            int row = (selectedIndex / columns - 1 + rows) % rows;
            selectedIndex = row * columns + col;
        }
        else if (input.y < -0.1f)
        {
            int col = selectedIndex % columns;
            int row = (selectedIndex / columns + 1) % rows;
            selectedIndex = row * columns + col;
        }

        if (selectedIndex >= totalSlots)
        {
            selectedIndex = previousIndex;
            return;
        }

        displayInventory.UnhighlightCell(previousIndex);
        displayInventory.CleanInteractionMenu();

        displayInventory.HighlightCell(selectedIndex);
        if (inventory.InventorySlots[selectedIndex].Item != null)
        {
            displayInventory.ShowInteractionMenu(GetInteractionHintsForSlot(selectedIndex));
        }
        else
        {
            displayInventory.HideInteractionMenu();
        }
    }

    protected virtual void ShowInventoryItem(BaseItemObject item)
    {
        if (isOpen)
        {
            displayInventory.RefreshUI(inventory.InventorySlots);
        }
    }
    public virtual bool TryPickupItem(BaseItemObject item, int amount)
    {
        bool success = inventory.SetEmptySlot(item, amount);
        if (!success)
        {
            Debug.Log("Inventory is full");
        }

        return success;
    }
    protected virtual List<InteractionHint> GetInteractionHintsForSlot(int slotIndex)
    {
        var slot = inventory.InventorySlots[slotIndex];
        var hints = new List<InteractionHint>();

        if (slot.Item == null) return hints;

        hints.Add(new InteractionHint { Description = "Drop", Key = inputActions.Player.Dash.GetBindingDisplayString() });
        hints.Add(new InteractionHint { Description = "Move", Key = inputActions.Player.Jump.GetBindingDisplayString() });

        if (slot.Item.CanEquip)  //need add intervace for equipable items
        {
            hints.Add(new InteractionHint
            {
                Description = slot.IsEquipped ? "Unequip" : "Equip",
                Key = inputActions.Player.Attack.GetBindingDisplayString()
            });
        }

        return hints;
    }

    protected virtual void OnDisable()
    {
        inventory.OnItemAdded -= ShowInventoryItem;
        inputActions.Player.Inventory.started -= ctx => ToggleInventory();
    }
}
