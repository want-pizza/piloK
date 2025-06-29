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
        inventory.OnItemEquiped += ShowEquippedItem;
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
    private void EnableInventoryInput()
    {
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Attack.started += OnInteract;
    }

    private void DisableInventoryInput()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Attack.started -= OnInteract;
    }
    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        HandleInventoryNavigation(input);
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        //HandleInventoryInteraction();
    }

    private void HandleInventoryNavigation(Vector2 input)
    {
        int columns = displayInventory.GetNumberSlotsInColumn;
        int totalSlots = inventory.CountSlots;
        int previousIndex = selectedIndex;

        if (input.x > 0.1f)
            selectedIndex += 1;
        else if (input.x < -0.1f)
            selectedIndex -= 1;
        else if (input.y > 0.1f)
            selectedIndex -= columns;
        else if (input.y < -0.1f)
            selectedIndex += columns;

        // Clamp
        selectedIndex = Mathf.Clamp(selectedIndex, 0, totalSlots - 1);

        if (previousIndex != selectedIndex)
            displayInventory.HighlightCell(selectedIndex);
    }


    protected virtual void ShowEquippedItem(BaseItemObject item)
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
    protected virtual void ShowInteractionForSlot(int slotIndex)
    {
        var slot = inventory.InventorySlots[slotIndex];
        if (slot.Item == null)
        {
            displayInventory.HideInteractionMenu();
            return;
        }

        var hints = new List<InteractionHint>
    {
        new InteractionHint { Description = "Drop", Key = KeyCode.Q },
        new InteractionHint { Description = "Move", Key = KeyCode.LeftShift }
    };

        if (slot.Item.CanEquip)
        {
            hints.Add(new InteractionHint
            {
                Description = slot.Item.IsEquipped ? "Unequip" : "Equip",
                Key = KeyCode.E
            });
        }

        displayInventory.ShowInteractionMenu(hints);
    }

    protected virtual void OnDisable()
    {
        inventory.OnItemEquiped -= ShowEquippedItem;
        inputActions.Player.Inventory.started -= ctx => ToggleInventory();
    }
}
