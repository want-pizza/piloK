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
    [SerializeField] private GameObject cell;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    private void OnEnable()
    {
        inventory.OnItemEquiped += UpdateUI;
    }
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    private void UpdateUI(BaseItemObject itemObject)
    {
        var obj = Instantiate(cell, Vector3.zero, Quaternion.identity, transform);
        Image image = obj.GetComponent<Image>();
        image.sprite = itemObject.Icon;
        obj.GetComponent<RectTransform>().localPosition = GetPosition(inventory.InventorySlots.Count-1);
        itemsDisplayed.Add(inventory.InventorySlots[inventory.InventorySlots.Count-1], obj);
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.InventorySlots.Count; i++) 
        {
            BaseItemObject itemObject = inventory.InventorySlots[i].Item;
            
            var obj = Instantiate(cell,Vector3.zero, Quaternion.identity, transform);
            Image image = obj.GetComponent<Image>();
            image.sprite = itemObject.Icon;
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            itemsDisplayed.Add(inventory.InventorySlots[i], obj);
        }
    }
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMN)), 0f);
    }
    private void OnDisable()
    {
        inventory.OnItemEquiped -= UpdateUI;
    }
}
