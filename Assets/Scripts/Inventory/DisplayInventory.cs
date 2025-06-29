using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int X_START;
    [SerializeField] private int Y_START;
    [SerializeField] private int X_SPACE_BETWEEN_ITEM;
    [SerializeField] private int Y_SPACE_BETWEEN_ITEMS;
    [SerializeField] private int NUMBER_SLOTS_IN_COLUMN;

    [SerializeField] private GameObject interactionMenu;
    private Dictionary<GameObject, InventorySlot> itemsDisplayed = new();

    public int GetNumberSlotsInColumn { get => NUMBER_SLOTS_IN_COLUMN; }

    public void CreateSlots(InventorySlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var cell = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity, transform);
            cell.GetComponent<RectTransform>().localPosition = GetPosition(i);
            itemsDisplayed.Add(cell, slots[i]);
        }
    }

    public void UpdateSlotUI(BaseItemObject itemObject)
    {
        foreach (var kvp in itemsDisplayed)
        {
            if (kvp.Value.Item == itemObject)
            {
                Image img = kvp.Key.GetComponent<Image>();
                img.sprite = itemObject.Icon;
                img.enabled = true;
                break;
            }
        }
    }

    public void RefreshUI(InventorySlot[] slots)
    {
        Debug.Log("RefreshUI");
        SelectFirstSlot();
        foreach (var kvp in itemsDisplayed)
        {
            Image img = kvp.Key.GetComponent<Image>();
            img.sprite = null;
            img.enabled = false;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            if (slot.Item != null)
            {
                foreach (var kvp in itemsDisplayed)
                {
                    if (kvp.Value.Id == slot.Id)
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
    public void SelectFirstSlot()
    {
        if (itemsDisplayed.Count == 0) return;
        Debug.Log("try select");
        HighlightCell(0);
    }

    public void HighlightCell(int index)
    {
        foreach (var kvp in itemsDisplayed) {
            Debug.Log($"id = {kvp.Value.Id}");
            if (kvp.Value.Id == index)
            {
                var outline = kvp.Key.GetComponent<Outline>();
                if (outline != null)
                {
                    Debug.Log("outline.enabled");
                    outline.enabled = true;
                }
            }
        }
       
    }

    public void ShowInteractionMenu(BaseItemObject item)
    {
        if (interactionMenu != null)
        {
            interactionMenu.SetActive(true);
        }
    }
    public void ShowInteractionMenu(List<InteractionHint> hints)
    { 
    
    }
    public void HideInteractionMenu()
    {
        interactionMenu?.SetActive(false);
    }
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_SLOTS_IN_COLUMN)),
                           Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_SLOTS_IN_COLUMN)),
                           0f);
    }
}
