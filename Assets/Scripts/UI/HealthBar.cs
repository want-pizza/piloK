using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI healthText;

    private float maxHealth, currentHealth;
    private float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            UpdateHealthBar();
        }
    }
    private float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            UpdateHealthBar();
        }
        
    }
    private void Start()
    {
        PlayerStats stats = FindAnyObjectByType<PlayerStats>();
        currentHealth = stats.CurrentHealth.Value;
        maxHealth = stats.MaxHealth.Value;

        stats.CurrentHealth.OnValueChanged += value => CurrentHealth = value; 
        stats.MaxHealth.OnValueChanged += value => MaxHealth = value; 
        UpdateHealthBar ();
    }
    void UpdateHealthBar()
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
