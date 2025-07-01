using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryPresenter : InventoryPresenterBase
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private System.Action<WeaponItemObject> OnWeaponEquiped;

    protected override void OnEnable()
    {
        Debug.Log("PlayerInventoryPresenter OnEnable");
        base.OnEnable();
        inventory.OnItemEquiped += RecalculateStats;
        isOpen = false;
        displayInventory.gameObject.SetActive(isOpen);
    }
    public void RecalculateStats(BaseItemObject itemObject)
    {
        if(itemObject is ISendModifires statData)
        {
            statData.Apply(playerStats);
        }
    }
    protected override void ToggleInventory()
    {
        isOpen = !isOpen;
        displayInventory.gameObject.SetActive(isOpen);

        if (isOpen)
        {
            displayInventory.RefreshUI(inventory.InventorySlots);
            InputManager.Instance.SwitchState(PlayerState.Inventory);
            Debug.Log("Inventory opened, enabling input");
            EnableInventoryInput();
            displayInventory.HighlightCell(selectedIndex);
            displayInventory.ShowInteractionMenu(GetInteractionHintsForSlot(selectedIndex));
        }
        else
        {
            InputManager.Instance.SwitchState(PlayerState.Normal);
            Debug.Log("Inventory closed, disabling input");
            DisableInventoryInput();
            displayInventory.UnhighlightCell(selectedIndex);
            displayInventory.CleanInteractionMenu();
            displayInventory.HideInteractionMenu();
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        inventory.OnItemEquiped -= RecalculateStats;
    }
    private void OnApplicationQuit()
    {
        inventory.InventorySlots = new InventorySlot[15];
    }
}
