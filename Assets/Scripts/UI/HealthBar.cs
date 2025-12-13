using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI healthText;
    public RectTransform holder;
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        fillImage.fillAmount = currentHealth / maxHealth;
        Debug.Log($"fillAmount = {fillImage.fillAmount}");

        healthText.text = (currentHealth < 0f ? "0" : currentHealth.ToString("#")) + " / " + maxHealth.ToString("#");

        float newWidth =
            maxHealth < 150 ? 150f :
            maxHealth > 200 ? maxHealth / 2 :
            maxHealth;

        holder.sizeDelta = new Vector2(newWidth, holder.sizeDelta.y);

    }
}
