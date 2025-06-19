using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private InventoryObject inventoryObject;
    [SerializeField] private DisplayInventory displayInventory;
    private bool isActive = true;

    PlayerAction inputActions;
    private void OnEnable()
    {
        inventoryObject.OnItemEquiped += RecalculateStats;
        inputActions = InputManager.Instance.PlayerActions;
        inputActions.Player.Inventory.started += ctx => InteractInventory();
        displayInventory.gameObject.SetActive(false);
    }
    public void TryPickupItem(BaseItemObject item, int amount)
    {
        Debug.Log($"TryPickupItem {item.name}, {amount}");
        inventoryObject.SetEmptySlot(item, amount);
    }
    public void RecalculateStats(BaseItemObject itemObject)
    {
        if(itemObject is ISendModifires statData)
        {
            statData.Apply(playerStats);
        }
    }
    private void InteractInventory()
    {
        displayInventory.gameObject.SetActive(isActive);
        isActive = !isActive;
    }
    private void OnDisable()
    {
        inventoryObject.OnItemEquiped -= RecalculateStats;
        inputActions.Player.Inventory.started -= ctx => InteractInventory();
    }
    private void OnApplicationQuit()
    {
        inventoryObject.InventorySlots = new InventorySlot[15];
    }
}
