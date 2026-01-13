using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    [SerializeField] private UnityEngine.GameObject cellPrefab;
    [SerializeField] private int X_START;
    [SerializeField] private int Y_START;
    [SerializeField] private int X_SPACE_BETWEEN_ITEM;
    [SerializeField] private int Y_SPACE_BETWEEN_ITEMS;
    [SerializeField] private int NUMBER_SLOTS_IN_COLUMN;

    [SerializeField] private UnityEngine.GameObject interactionMenu;
    [SerializeField] private UnityEngine.GameObject hintTemplate;
    [SerializeField] private GameObject itemDescriptionHolder;
    [SerializeField] private TMP_Text itemDescriptionText;

    private List<UnityEngine.GameObject> cells = new();

    public int GetNumberSlotsInColumn { get => NUMBER_SLOTS_IN_COLUMN; }

    public void CreateSlots(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            var cell = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity, transform);
            cell.GetComponent<RectTransform>().localPosition = GetPosition(i);
            cells.Add(cell);
        }
    }

    public void RefreshUI(InventorySlot[] slots)
    {
        Debug.Log("RefreshUI");
        foreach (var cell in cells)
        {
            Image img = cell.GetComponent<Image>();
            img.sprite = null;
            img.enabled = false;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            if (slot.Item != null)
            {
                Image img = cells[i].GetComponent<Image>();
                img.sprite = slot.Item.Icon;
                img.enabled = true;
            }
        }
    }

    public void HighlightCell(int index)
    {
        var outline = cells[index].GetComponentInChildren<Outline>();
        if (outline != null)
        {
            //Debug.Log($" outline.effectColor - {outline.effectColor}");
            outline.enabled = true;
        }
    }
    public void UnhighlightCell(int index)
    {
        var outline = cells[index].GetComponentInChildren<Outline>();
        if (outline != null)
        {
            //Debug.Log($" outline.effectColor - {outline.effectColor}");
            outline.enabled = false;
        }
    }
    public void ShowInteractionMenu(List<InteractionHint> hints)
    {
        if (interactionMenu == null) return;

        interactionMenu.SetActive(true);

        foreach (var hint in hints)
        {
            UnityEngine.GameObject hintGO = Instantiate(hintTemplate, interactionMenu.transform);
            hintGO.SetActive(true);

            var text = hintGO.GetComponent<TMPro.TextMeshProUGUI>();
            if (text != null)
            {
                text.text = $"[{hint.Key}] {hint.Description}";
            }
        }
    }
    internal void ClearInteractionMenu()
    {
        foreach (Transform child in interactionMenu.transform)
            Destroy(child.gameObject);
    }
    public void HideInteractionMenu()
    {
        interactionMenu?.SetActive(false);
    }

    public void ShowDescriptionWindow(string text)
    {
        itemDescriptionText.text = text;
        itemDescriptionHolder.SetActive(true);
    }
    public void HideDescriptionWindow()
    {
        itemDescriptionHolder.SetActive(false);
    }
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_SLOTS_IN_COLUMN)),
                           Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_SLOTS_IN_COLUMN)),
                           0f);
    }
}
