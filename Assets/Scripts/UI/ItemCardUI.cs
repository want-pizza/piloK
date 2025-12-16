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

        backgroundImage.color = GetColorByRareness(data.Rareness);

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
            case ItemRareness.Usual: return Color.white;
            case ItemRareness.Rare: return Color.blue;
            case ItemRareness.Epic: return Color.magenta;
            case ItemRareness.Legendary: return Color.yellow;
            default: return Color.gray;
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
