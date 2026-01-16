using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryPresenter: InventoryPresenterBase, ICanBePaused
{
    [SerializeField] private PlayerContext playerContext;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerMovement playerMovement;

    #region Unity lifecycle

    protected override void OnEnable()
    {
        base.OnEnable();

        inventory.OnItemEquiped += OnItemEquipped;
        inventory.OnItemUnequiped += OnItemUnequipped;
        PauseManager.OnPauseChanged += OnPausedChanged;

        isOpen.Value = false;
        displayInventory.gameObject.SetActive(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        inventory.OnItemEquiped -= OnItemEquipped;
        inventory.OnItemUnequiped -= OnItemUnequipped;
        PauseManager.OnPauseChanged -= OnPausedChanged;
    }

    private void OnApplicationQuit()
    {
        inventory.InventorySlots = new InventorySlot[15];
    }

    #endregion

    #region Pause handling

    public void OnPausedChanged(bool paused)
    {
        ToggleInventory(paused);
    }

    #endregion

    #region Inventory open / close overrides

    protected override void OpenInventory()
    {
        base.OpenInventory();
        SelectCell();
    }

    protected override void CloseInventory()
    {
        base.CloseInventory();

        displayInventory.UnhighlightCell(selectedIndex);
        displayInventory.ClearInteractionMenu();
        displayInventory.HideInteractionMenu();
    }

    protected void ToggleInventory(bool value)
    {
        if (isOpen.Value == value)
            return;

        isOpen.Value = value;
        displayInventory.gameObject.SetActive(isOpen);

        if (isOpen)
            OpenInventory();
        else
            CloseInventory();
    }

    #endregion

    #region Equip / Unequip

    protected override void TryEquip(InputAction.CallbackContext ctx)
    {
        bool changed;

        if (inventory.SlotIsEquiped(selectedIndex))
            changed = inventory.UnequipItem(selectedIndex);
        else
            changed = inventory.EquipItem(selectedIndex);

        if (changed)
            RefreshInteractionMenu();
    }

    private void OnItemEquipped(BaseItemObject item)
    {
        ApplyItemEffects(item);
    }

    private void OnItemUnequipped(BaseItemObject item)
    {
        RemoveItemEffects(item);
    }

    private void ApplyItemEffects(BaseItemObject item)
    {
        if (item is ISendModifires modifiers)
            modifiers.Apply(playerStats);

        if (item is RuntimeItemData runtimeItem)
            runtimeItem.OnEquip(playerContext);
    }

    private void RemoveItemEffects(BaseItemObject item)
    {
        if (item is ISendModifires modifiers)
            modifiers.Remove(playerStats);

        if (item is RuntimeItemData runtimeItem)
            runtimeItem.OnUnequip();
    }

    #endregion

    #region UI helpers

    protected void RefreshInteractionMenu()
    {
        displayInventory.ClearInteractionMenu();
        displayInventory.ShowInteractionMenu(
            GetInteractionHintsForSlot(selectedIndex)
        );
    }

    public void RefreshUI()
    {
        displayInventory.RefreshUI(inventory.InventorySlots);
    }

    #endregion
}
