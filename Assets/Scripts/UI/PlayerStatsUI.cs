using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [Header("Basic stats")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI resistTimeText;
    [SerializeField] private TextMeshProUGUI physicalDamageText;
    [SerializeField] private TextMeshProUGUI fireDamageText;

    private void Start()
    {
        // Підписуємося на зміни
        playerStats.MaxHealth.OnValueChanged += _ => UpdateUI();
        playerStats.CurrentHealth.OnValueChanged += _ => UpdateUI();
        playerStats.ResistTime.OnValueChanged += _ => UpdateUI();
        playerStats.PhisicalDamage.OnValueChanged += _ => UpdateUI();
        playerStats.FireDamage.OnValueChanged += _ => UpdateUI();

        UpdateUI(); // одноразове оновлення при старті
    }

    private void UpdateUI()
    {
        healthText.text = $"HP: {playerStats.CurrentHealth.Value}/{playerStats.MaxHealth.Value}";
        resistTimeText.text = $"Resist: {playerStats.ResistTime.Value}s";
        physicalDamageText.text = $"Phys DMG: {playerStats.PhisicalDamage.Value}";
        fireDamageText.text = $"Fire DMG: {playerStats.FireDamage.Value}";
    }
}
