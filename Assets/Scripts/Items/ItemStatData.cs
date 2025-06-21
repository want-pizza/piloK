using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Stat Item")]
public class ItemStatData : BaseItemObject, ISendModifires
{
    public List<StatBonus> bonuses;

    public void Apply(PlayerStats stats)
    {
        foreach (var bonus in bonuses)
        {
            switch (bonus.statType)
            {
                case StatType.MaxHealth:
                    stats.MaxHealth.AddModifier(new AddModifier<float>(bonus.amount));
                    break;
                case StatType.Damage:
                    stats.Damage.AddModifier(new AddModifier<float>(bonus.amount));
                    break;
            }
        }
    }
}
