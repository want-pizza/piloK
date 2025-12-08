using QuantumTek.SimpleMenu;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class LevelUpItemChoiceUI : MonoBehaviour
{
    [SerializeField] private ItemCardUI itemCardPrefab;
    [SerializeField] private Transform itemsContainer;

    private System.Action<ItemStatData> onItemSelected;

    public void Open(ItemStatData[] items, System.Action<ItemStatData> callback)
    {
        GetComponent<SM_Window>().Toggle(true);
        onItemSelected = callback;

        foreach (Transform child in itemsContainer)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            var card = Instantiate(itemCardPrefab, itemsContainer);
            card.Setup(item, SelectItem);
        }
    }

    private void SelectItem(ItemStatData item)
    {
        onItemSelected?.Invoke(item);
        Close();
    }

    public void Close() => gameObject.SetActive(false);
}
