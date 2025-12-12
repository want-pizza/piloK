using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerStatsUI statsUI;

    public void ShowStats()
    {
        var stats = playerStats.GetAllStats();
        statsUI.DisplayStats(stats);
    }

    public void ShowItemPreview(ItemStatData item)
    {
        var previews = new List<StatPreview>();

        foreach (var bonus in item.bonuses)
        {
            Stat<float> stat = playerStats.GetAllStats()[bonus.statType];

            float oldValue = stat.Value;


            float newValue = StatPreviewUtility.CalculatePreviewValue(stat, bonus);

            previews.Add(new StatPreview(
                bonus.statType,
                oldValue,
                newValue
            ));
        }

        statsUI.ShowPreviews(previews);
    }

    public void HidePreview()
    {
        statsUI.ClearAllPreviews();
    }

}
