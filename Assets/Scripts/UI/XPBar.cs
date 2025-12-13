using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBar : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI xpText;
    public RectTransform holder;

    private int maxXP;
    private int currentXP;

    private int MaxXP
    {
        get => maxXP;
        set
        {
            maxXP = value;
            UpdateXPBar();
        }
    }
    private int CurrentXP
    {
        get => currentXP;
        set
        {
            currentXP = value;
            UpdateXPBar();
        }
    }

    private void Start()
    {
        PlayerLevel level = FindAnyObjectByType<PlayerLevel>();

        currentXP = level.CurrentXP.Value;
        maxXP = level.GetXPToNextLevel();

        level.CurrentXP.OnValueChanged += xp => CurrentXP = xp;
        level.CurrentLevel.OnValueChanged += _ => MaxXP = level.GetXPToNextLevel();

        UpdateXPBar();
    }

    private void UpdateXPBar()
    {
        float fill = (float)currentXP / maxXP;
        fillImage.fillAmount = Mathf.Clamp01(fill);

        xpText.text = $"{currentXP} / {maxXP}";

        float newWidth =
            maxXP < 50 ? 150f :
            maxXP + 100f;

        holder.sizeDelta = new Vector2(newWidth, holder.sizeDelta.y);
    }
}