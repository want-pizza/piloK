using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI healthText;
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        fillImage.fillAmount = currentHealth / maxHealth;
        Debug.Log($"fillAmount = {fillImage.fillAmount}");

        healthText.text = (currentHealth < 0f ? "0" : currentHealth.ToString("#")) + " / " + maxHealth.ToString("#");

        RectTransform rt = GetComponent<RectTransform>();

        float newWidth =
            maxHealth < 150 ? 150f :
            maxHealth > 200 ? maxHealth / 2 :
            maxHealth;

        rt.sizeDelta = new Vector2(newWidth, rt.sizeDelta.y);

    }
}
