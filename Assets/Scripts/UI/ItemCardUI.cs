using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    //[SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Transform bonusesParent;
    [SerializeField] private TMP_Text bonusPrefab;

    private PlayerStatsController controller;

    private ItemStatData itemData;
    private System.Action<ItemStatData> onSelectCallback;

    public void Setup(PlayerStatsController controller, ItemStatData data, System.Action<ItemStatData> callback)
    {
        this.controller = controller;
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
        controller.HidePreview();

        Debug.Log($"onSelectCallback = {onSelectCallback}");
        onSelectCallback?.Invoke(itemData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        controller.ShowItemPreview(itemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        controller.HidePreview();
    }
}
