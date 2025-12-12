using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private StatUIElement statElementPrefab;

    private readonly Dictionary<StatType, StatUIElement> pool =
        new Dictionary<StatType, StatUIElement>();

    private readonly Dictionary<StatType, string> prettyNames = new Dictionary<StatType, string>()
    {
        { StatType.PhisicalDamage, "STR" },
        { StatType.MaxHealth, "HLT" },
        { StatType.CritChance, "CCH" },
        { StatType.CritMultiplier, "CML" },
        { StatType.VampirismChance, "VCH" },
        { StatType.VampirismStrength, "VSTR" }
    };

    // ------------------------------
    // MAIN DISPLAY
    // ------------------------------
    public void DisplayStats(Dictionary<StatType, Stat<float>> stats)
    {
        foreach (var pair in stats)
        {
            if (!prettyNames.ContainsKey(pair.Key))
                continue;

            StatUIElement element;

            // if exists, reuse
            if (pool.TryGetValue(pair.Key, out element))
            {
                element.gameObject.SetActive(true);
            }
            else
            {
                element = Instantiate(statElementPrefab, container);
                pool[pair.Key] = element;
            }

            element.SetName(prettyNames[pair.Key]);
            element.SetValue(pair.Value.Value);
        }

        // hide elements that are NOT present in stats
        foreach (var kvp in pool)
        {
            if (!stats.ContainsKey(kvp.Key))
                kvp.Value.gameObject.SetActive(false);
        }
    }

    // ------------------------------
    // PREVIEW
    // ------------------------------
    public void ShowPreviews(IEnumerable<StatPreview> previews)
    {
        foreach (var preview in previews)
        {
            if (pool.TryGetValue(preview.Type, out var ui))
                ui.ShowPreview(preview.Delta, preview.NewValue);
        }
    }

    public void ClearAllPreviews()
    {
        foreach (var ui in pool.Values)
            ui.ClearPreview();
    }
}
