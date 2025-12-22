using QuantumTek.SimpleMenu;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using System.Collections.Generic;

public class LevelUpItemChoiceUI : MonoBehaviour
{
    [SerializeField] private PlayerInventoryPresenter inventoryPresenter;
    [SerializeField] private PlayerStatsController statsController;
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private ItemCardUI itemCardPrefab;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private AudioClip rerollCancled;

    public void Open(List<BaseItemObject> items)
    {
        inventoryPresenter.RefreUI();
        statsController.ShowStats();
        GetComponent<SM_Window>().Toggle(true);

        foreach (Transform child in itemsContainer)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            var card = Instantiate(itemCardPrefab, itemsContainer);
            card.SetupItemStatData(statsController, item, SelectItem);
        }
    }

    private void SelectItem(BaseItemObject item)
    {
        Debug.Log("inventoryPresenter.TryPickupItem");
        inventoryPresenter.TryPickupItem(item, 1);
        PauseController.Instance.SetPause(false);
        Close();
    }

    public void TryReroll()
    {
        if (!playerLevel.Reroll())
        {
            AudioManager.Instance.PlaySFX(rerollCancled);
        }
    }

    public void Close() => GetComponent<SM_Window>().Toggle(false);
}
