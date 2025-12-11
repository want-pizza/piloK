using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private StatUIElement statElementPrefab;

    private readonly List<StatUIElement> pool = new List<StatUIElement>();

    private Dictionary<StatType, string> prettyNames = new Dictionary<StatType, string>()
    {
        { StatType.PhisicalDamage, "STR" },
        { StatType.MaxHealth, "HLT" },
        { StatType.CritChance, "CCH" },
        { StatType.CritMultiplier, "CML" },
        { StatType.VampirismChance, "VCH" },
        { StatType.VampirismStrength, "VSTR" }
    };

    public void DisplayStats(Dictionary<StatType, Stat<float>> stats)
    {
        int index = 0;

        foreach (var pair in stats)
        {
            if (!prettyNames.ContainsKey(pair.Key))
                continue;

            StatUIElement element;

            if (index < pool.Count)
            {
                element = pool[index];
                element.gameObject.SetActive(true);
            }
            else
            {
                element = Instantiate(statElementPrefab, container);
                pool.Add(element);
            }

            element.SetName(prettyNames[pair.Key]);
            element.SetValue(pair.Value.Value);

            index++;
        }

        for (int i = index; i < pool.Count; i++)
            pool[i].gameObject.SetActive(false);
    }
}
