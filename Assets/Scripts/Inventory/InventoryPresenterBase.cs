using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InventoryPresenterBase : MonoBehaviour
{
    [SerializeField] protected InventoryObject inventory;
    [SerializeField] protected DisplayInventory displayInventory;

    protected PlayerAction inputActions;

    protected readonly Field<bool> isOpen = new();
    protected int selectedIndex;
    protected int previousIndex;

    protected bool itemIsMoving;
    protected int movingItemIndex;

    public Field<bool> IsOpen => isOpen;

    #region Unity lifecycle

    protected virtual void Awake()
    {
        inputActions = InputManager.Instance.PlayerActions;
        InitializeInventory();
    }

    protected virtual void OnEnable()
    {
        inventory.OnItemAdded += ShowInventoryItem;
    }

    protected virtual void OnDisable()
    {
        inventory.OnItemAdded -= ShowInventoryItem;
        UnbindInput();
    }

    #endregion

    #region Inventory initialization

    private void InitializeInventory()
    {
        for (int i = 0; i < inventory.CountSlots; i++)
        {
            inventory.InventorySlots[i] = new InventorySlot(i, null, 0);
        }

        displayInventory.CreateSlots(inventory.InventorySlots.Length);
    }

    #endregion

    #region Inventory open / close

    protected virtual void ToggleInventory(InputAction.CallbackContext ctx)
    {
        isOpen.Value = !isOpen;
        displayInventory.gameObject.SetActive(isOpen);

        if (isOpen)
            OpenInventory();
        else
            CloseInventory();
    }

    protected virtual void OpenInventory()
    {
        displayInventory.RefreshUI(inventory.InventorySlots);
        BindInput();
    }

    protected virtual void CloseInventory()
    {
        UnbindInput();
        itemIsMoving = false;
    }

    #endregion

    #region Input binding

    protected virtual void BindInput()
    {
        inputActions.Player.Move.performed += MoveInCells;
        inputActions.Player.Jump.started += MoveItem;
        inputActions.Player.Attack.started += TryEquip;
        inputActions.Player.Dash.canceled += TryDrop;
    }

    protected virtual void UnbindInput()
    {
        inputActions.Player.Move.performed -= MoveInCells;
        inputActions.Player.Jump.started -= MoveItem;
        inputActions.Player.Attack.started -= TryEquip;
        inputActions.Player.Dash.canceled -= TryDrop;
    }

    #endregion

    #region Item interaction

    protected virtual void MoveItem(InputAction.CallbackContext ctx)
    {
        if (!itemIsMoving)
        {
            if (inventory.InventorySlots[selectedIndex].Item == null)
                return;

            movingItemIndex = selectedIndex;
            itemIsMoving = true;
            inputActions.Player.Attack.started -= TryEquip;
        }
        else
        {
            inventory.SwapItems(movingItemIndex, selectedIndex);
            itemIsMoving = false;
            inputActions.Player.Attack.started += TryEquip;
            displayInventory.RefreshUI(inventory.InventorySlots);
        }
    }

    protected virtual void TryEquip(InputAction.CallbackContext ctx)
    {
        if (inventory.SlotIsEquiped(selectedIndex))
        {
            if(!inventory.UnequipItem(selectedIndex))
                ErrorHandler.Instance.PlayErrorNoArgs();

            return;
        }

        if (!inventory.EquipItem(selectedIndex))
            ErrorHandler.Instance.PlayErrorNoArgs();
    }

    protected virtual void TryDrop(InputAction.CallbackContext ctx)
    {
        if (inventory.DropItem(selectedIndex))
            displayInventory.RefreshUI(inventory.InventorySlots);

        else ErrorHandler.Instance.PlayErrorNoArgs();
    }

    #endregion

    #region Navigation

    private void MoveInCells(InputAction.CallbackContext ctx)
    {
        HandleInventoryNavigation(ctx.ReadValue<Vector2>());
    }

    protected void HandleInventoryNavigation(Vector2 input)
    {
        int columns = displayInventory.GetNumberSlotsInColumn;
        int totalSlots = inventory.CountSlots;
        int rows = Mathf.CeilToInt((float)totalSlots / columns);

        previousIndex = selectedIndex;

        if (input.x > 0.1f)
            selectedIndex += 1;
        else if (input.x < -0.1f)
            selectedIndex -= 1;
        else if (input.y > 0.1f)
            selectedIndex -= columns;
        else if (input.y < -0.1f)
            selectedIndex += columns;

        if (selectedIndex < 0 || selectedIndex >= totalSlots)
        {
            selectedIndex = previousIndex;
            return;
        }

        SelectCell();
    }

    protected void SelectCell()
    {
        displayInventory.UnhighlightCell(previousIndex);
        displayInventory.HighlightCell(selectedIndex);

        displayInventory.ClearInteractionMenu();

        var slot = inventory.InventorySlots[selectedIndex];

        if (slot.Item == null)
        {
            displayInventory.HideInteractionMenu();
            displayInventory.HideDescriptionWindow();
            return;
        }

        displayInventory.ShowInteractionMenu(GetInteractionHintsForSlot(selectedIndex));
        displayInventory.ShowDescriptionWindow(slot.Item.Description);
    }

    #endregion

    #region External API

    protected virtual void ShowInventoryItem(BaseItemObject item)
    {
        if (isOpen)
            displayInventory.RefreshUI(inventory.InventorySlots);
    }

    public virtual bool TryPickupItem(BaseItemObject item, int amount)
    {
        return inventory.SetEmptySlot(item, amount);
    }

    public virtual bool IsItemInInventory(BaseItemObject item)
    {
        return inventory.IsEquipped(item);
    }

    #endregion

    #region Interaction hints

    protected virtual List<InteractionHint> GetInteractionHintsForSlot(int slotIndex)
    {
        var slot = inventory.InventorySlots[slotIndex];
        var hints = new List<InteractionHint>();

        if (slot.Item == null)
            return hints;

        hints.Add(new InteractionHint
        {
            Description = "Drop",
            Key = inputActions.Player.Dash.GetBindingDisplayString()
        });

        hints.Add(new InteractionHint
        {
            Description = "Move",
            Key = inputActions.Player.Jump.GetBindingDisplayString()
        });

        if (slot.Item.CanEquip)
        {
            hints.Add(new InteractionHint
            {
                Description = slot.IsEquipped ? "Unequip" : "Equip",
                Key = inputActions.Player.Attack.GetBindingDisplayString()
            });
        }

        return hints;
    }

    #endregion
}
