using UnityEngine;

public class PlayerStatsAutoUI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Transform container;
    [SerializeField] private StatUIElement statUIPrefab;

    private void Start()
    {
        BuildUI();
    }

    private void BuildUI()
    {
        foreach (var pair in playerStats.GetAllStats())
        {
            StatType statType = pair.Key;
            Stat<float> stat = pair.Value;

            // створюємо UI елемент
            var ui = Instantiate(statUIPrefab, container);

            // назва з enum
            ui.SetName(statType.ToString());

            // початкове значення
            ui.SetValue(stat.Value);

            // підписуємося на зміни
            stat.OnValueChanged += value => ui.SetValue(value);
        }
    }
}
