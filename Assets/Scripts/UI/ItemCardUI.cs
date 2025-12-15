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

        foreach (Transform child in bonusesParent)
            Destroy(child.gameObject);

        TMP_Text txt = Instantiate(bonusPrefab, bonusesParent);
        txt.text += $"{baseItemData.Description}";
        txt.enabled = true;
        Debug.Log($"Instantiated Bonus active={txt.gameObject.activeSelf} enabled={txt.enabled}", txt.gameObject);
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
