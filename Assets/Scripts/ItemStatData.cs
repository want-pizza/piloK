using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Stat Item")]
public class ItemStatData : BaseItemData
{
    public List<StatBonus> bonuses;

    public override void OnPickup(PlayerStats playerStats)
    {
        Debug.Log("Trying apply stats");
        Apply(playerStats);
    }

    public override void Apply(PlayerStats stats)
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
