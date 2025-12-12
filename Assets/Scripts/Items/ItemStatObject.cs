using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Stat Item")]
public class ItemStatData : BaseItemObject, ISendModifires
{
    public List<StatBonus> bonuses;
    private Dictionary<StatType, IStatModifier<float>> modifiers = new Dictionary<StatType, IStatModifier<float>>(); // List<IStatModifier<float>> if want to have many modifiers to one stat

    public void Apply(PlayerStats stats)
    {
        for(int i = 0; i < bonuses.Count; i++)
        {
            StatType statType = bonuses[i].statType;
            IStatModifier<float> modifier = ModifierFactory.GetModifier(bonuses[i].modifier, bonuses[i].amount);
            
            stats.GetStatByType(statType).AddModifier(modifier);
            modifiers.Add(statType, modifier);
        }
    }
    public void Remove(PlayerStats stats)
    {
        for (int i = 0; i < bonuses.Count; i++)
        {
            StatType statType = bonuses[i].statType;

            stats.GetStatByType(statType).RemoveModifier(modifiers[statType]);
            modifiers.Remove(statType);
        }
    }
}
