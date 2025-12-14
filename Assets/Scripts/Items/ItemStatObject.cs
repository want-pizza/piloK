using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Stat Item")]
public class ItemStatData : BaseItemObject, ISendModifires
{
    public List<StatBonus> bonuses;
    private Dictionary<StatType, List<IStatModifier<float>>> modifiers = new Dictionary<StatType, List<IStatModifier<float>>>(); // List<IStatModifier<float>> if want to have many modifiers to one stat

    public void Apply(PlayerStats stats)
    {
        for(int i = 0; i < bonuses.Count; i++)
        {
            StatType statType = bonuses[i].statType;
            IStatModifier<float> modifier = ModifierFactory.GetModifier(bonuses[i].modifier, bonuses[i].amount);
            
            stats.GetStatByType(statType).AddModifier(modifier);

            if (!modifiers.TryGetValue(statType, out var list))
            {
                list = new List<IStatModifier<float>>();
                modifiers.Add(statType, list);
            }

            list.Add(modifier);
        }
    }
    public void Remove(PlayerStats stats)
    {
        foreach (var pair in modifiers)
        {
            var stat = stats.GetStatByType(pair.Key);
            var list = pair.Value;

            for (int i = 0; i < list.Count; i++)
            {
                stat.RemoveModifier(list[i]);
            }
        }

        modifiers.Clear();
    }
}
