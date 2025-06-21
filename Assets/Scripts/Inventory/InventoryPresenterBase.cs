using UnityEngine;

public abstract class InventoryPresenterBase : MonoBehaviour
{
    [SerializeField] protected InventoryObject inventory;
    [SerializeField] protected DisplayInventory displayInventory;

    protected PlayerAction inputActions;
    protected bool isOpen;

    protected virtual void Awake()
    {
        inputActions = InputManager.Instance.PlayerActions;
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
        }
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
    protected virtual void OnDisable()
    {
        inventory.OnItemEquiped -= ShowEquippedItem;
        inputActions.Player.Inventory.started -= ctx => ToggleInventory();
    }
}
