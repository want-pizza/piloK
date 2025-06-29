using UnityEngine;
using UnityEngine.UI;

public class InteractionMenuController : MonoBehaviour
{
    [SerializeField] private Text itemNameText;
    [SerializeField] private Image itemIconImage;

    public void SetItem(BaseItemObject item)
    {
        itemNameText.text = item.name;
        itemIconImage.sprite = item.Icon;
    }
}
