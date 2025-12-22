using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class ItemCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text rarenessText;
    [SerializeField] private Image backgroundImage;
    //[SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Transform bonusesParent;
    [SerializeField] private TMP_Text bonusPrefab;

    private PlayerStatsController controller;

    private BaseItemObject baseItemData;
    private System.Action<BaseItemObject> onSelectCallback;

    public void SetupItemStatData(PlayerStatsController controller, BaseItemObject data, System.Action<BaseItemObject> callback)
    {
        this.controller = controller;
        baseItemData = data;
        onSelectCallback = callback;

        icon.sprite = data.Icon;
        nameText.text = data.DisplayName;
        //descriptionText.text = data.Description;

        rarenessText.text = GetNameByRareness(data.Rareness);
        rarenessText.color = GetColorByRareness(data.Rareness);

        foreach (Transform child in bonusesParent)
            Destroy(child.gameObject);

        TMP_Text txt = Instantiate(bonusPrefab, bonusesParent);
        txt.text += $"{baseItemData.Description}";
        txt.enabled = true;
        Debug.Log($"Instantiated Bonus active={txt.gameObject.activeSelf} enabled={txt.enabled}", txt.gameObject);
    }

    private Color GetColorByRareness(ItemRareness rareness)
    {
        switch (rareness)
        {
            case ItemRareness.Usual: return new Color(201, 201, 201, 10);
            case ItemRareness.Rare: return new Color(76, 175, 80, 12);
            case ItemRareness.Epic: return new Color(155, 89, 182, 21);
            case ItemRareness.Legendary: return new Color(245, 181, 26);
            default: return Color.gray;
        }
    }
    private string GetNameByRareness(ItemRareness rareness)
    {
        switch (rareness)
        {
            case ItemRareness.Usual: return "Usual";
            case ItemRareness.Rare: return "Rare";
            case ItemRareness.Epic: return "Epic";
            case ItemRareness.Legendary: return "Legendary";
            default: return "none";
        }
    }

    public void OnClick()
    {
        controller.HidePreview();

        Debug.Log($"onSelectCallback = {onSelectCallback}");
        onSelectCallback?.Invoke(baseItemData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        controller.ShowItemPreview(baseItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        controller.HidePreview();
    }
}
