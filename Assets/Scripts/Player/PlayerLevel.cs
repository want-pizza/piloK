using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private LevelUpItemChoiceUI ui;
    [SerializeField] private List<ItemStatData> allItems;
    [SerializeField] private ItemStatData[] items;

    private void Start()
    {
        OnLevelUp();
    }
    void OnLevelUp()
    {
        ui.Open(items, OnItemSelected);
    }

    void OnItemSelected(ItemStatData item)
    {
        item.Apply(GetComponent<PlayerStats>());
    }
}