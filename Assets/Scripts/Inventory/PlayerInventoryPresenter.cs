using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryPresenter : InventoryPresenterBase
{
    [SerializeField] private PlayerStats playerStats;

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
