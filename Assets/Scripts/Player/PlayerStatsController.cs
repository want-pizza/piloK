using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour, ICanBePaused
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerStatsUI statsUI;

    private void OnEnable()
    {
        Subscribe();
        PauseManager.OnPauseChanged += OnPausedChanged;
    }
    private void OnDisable()
    {
        Unsubscribe();
        PauseManager.OnPauseChanged -= OnPausedChanged;
    }

    private void Subscribe()
    {
        foreach (var stat in playerStats.GetAllStats().Values)
        {
            stat.OnValueChangedNoArgs += ShowStats;
        }
    }

    private void Unsubscribe()
    {
        foreach (var stat in playerStats.GetAllStats().Values)
        {
            stat.OnValueChangedNoArgs -= ShowStats;
        }
    }

    public void ShowStats(bool value)
    {
        statsUI.gameObject.SetActive(value);

        if (value)
        {
            var stats = playerStats.GetAllStats();
            statsUI.DisplayStats(stats);
        }
    }

    public void ShowStats()
    {
        var stats = playerStats.GetAllStats();
        statsUI.DisplayStats(stats);
    }

    public void ShowItemPreview(BaseItemObject _item)
    {
        ItemStatData item = _item is ItemStatData ? (ItemStatData)_item : null;

        if (item == null)
            return;

        var previews = new List<StatPreview>();

        var groupedBonuses = new Dictionary<StatType, List<StatBonus>>();

        foreach (var bonus in item.bonuses)
        {
            if (!groupedBonuses.TryGetValue(bonus.statType, out var list))
            {
                list = new List<StatBonus>();
                groupedBonuses.Add(bonus.statType, list);
            }

            list.Add(bonus);
        }

        foreach (var pair in groupedBonuses)
        {
            StatType statType = pair.Key;
            List<StatBonus> bonuses = pair.Value;

            Stat<float> stat = playerStats.GetStatByType(statType);

            float oldValue = stat.Value;
            float newValue = StatPreviewUtility.CalculatePreviewValue(stat, bonuses);

            previews.Add(new StatPreview(
                statType,
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

    public void OnPausedChanged(bool paused)
    {
        ShowStats(paused);
    }
}
