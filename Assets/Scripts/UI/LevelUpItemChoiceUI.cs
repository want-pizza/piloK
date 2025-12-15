using QuantumTek.SimpleMenu;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class LevelUpItemChoiceUI : MonoBehaviour
{
    [SerializeField] private PlayerInventoryPresenter inventoryPresenter;
    [SerializeField] private PlayerStatsController statsController;
    [SerializeField] private ItemCardUI itemCardPrefab;
    [SerializeField] private Transform itemsContainer;

    public void Open(BaseItemObject[] items)
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

    public void Close() => GetComponent<SM_Window>().Toggle(false);
}
