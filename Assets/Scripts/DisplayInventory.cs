using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_COLUMN;
    [SerializeField] private GameObject cellPrefab;
    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
    private void OnEnable()
    {
        inventory.OnItemEquiped += UpdateUI;
        RefreshUI();
    }
    private void Awake()
    {
        CreateSlots();
    }

    public void UpdateUI(BaseItemObject itemObject)
    {
        Debug.Log("UpdateUI");
        for (int i = 0; i < inventory.InventorySlots.Length; i++)
        {
            var slot = inventory.InventorySlots[i];
            if (slot.Item == itemObject)
            {
                foreach (var kvp in itemsDisplayed)
                {
                    Debug.Log($"kvp.Value - {kvp.Value}");
                    if (kvp.Value == slot)
                    {
                        Image img = kvp.Key.GetComponent<Image>();
                        img.sprite = itemObject.Icon;
                        img.enabled = true;
                        break;
                    }
                }
            }
        }
    }
    public void CreateSlots()
    {
        for (int i = 0; i < inventory.InventorySlots.Length; i++)
        {
            var cell = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity, transform);
            cell.GetComponent<RectTransform>().localPosition = GetPosition(i);

            itemsDisplayed.Add(cell, inventory.InventorySlots[i]);
        }
    }
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0f);
    }

    public void RefreshUI()
    {
        for (int i = 0; i < inventory.InventorySlots.Length; i++)
        {
            var slot = inventory.InventorySlots[i];
            if (slot.Item != null)
            {
                foreach (var kvp in itemsDisplayed)
                {
                    if (kvp.Value == slot)
                    {
                        Image img = kvp.Key.GetComponent<Image>();
                        img.sprite = slot.Item.Icon;
                        img.enabled = true;
                        break;
                    }
                }
            }
        }
    }
    private void OnDisable()
    {
        inventory.OnItemEquiped -= UpdateUI;
    }
}
