using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCardUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    //[SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Transform bonusesParent;
    [SerializeField] private TMP_Text bonusPrefab;

    private ItemStatData itemData;
    private System.Action<ItemStatData> onSelectCallback;

    public void Setup(ItemStatData data, System.Action<ItemStatData> callback)
    {
        itemData = data;
        onSelectCallback = callback;

        icon.sprite = data.Icon;
        nameText.text = data.DisplayName;
        //descriptionText.text = data.Description;

        foreach (Transform child in bonusesParent)
            Destroy(child.gameObject);

        foreach (var bonus in data.bonuses)
        {
            TMP_Text txt = Instantiate(bonusPrefab, bonusesParent);
            txt.text += $"{bonus.statType}: +{bonus.amount}";
            txt.enabled = true;
            Debug.Log($"Instantiated Bonus active={txt.gameObject.activeSelf} enabled={txt.enabled}", txt.gameObject);
        }
    }

    public void OnClick()
    {
        onSelectCallback?.Invoke(itemData);
    }
}
