using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class DamageBuilder
{
    public static List<DamageInfo> BuildForPlayer(PlayerStats stats)
    {
        if(stats == null)
        {
            Debug.LogWarning("PlayerStat == null");
            return null;
        }
        var list = new List<DamageInfo>();

        if (stats.PhisicalDamage != 0)
        {
            float finalDamage = stats.PhisicalDamage;
            CalculateCrit(ref finalDamage, stats.CritChance, stats.CritMultiplier);

            DamageInfo info = new DamageInfo
            {
                Amount = stats.PhisicalDamage,
                Type = DamageType.Physical,
                Attacker = stats.gameObject,
                IsCritical = stats.PhisicalDamage != finalDamage,
                KnockBackForce = stats.KnockBackForce
            };

            list.Add(info);
        }

        if (stats.FireDamage != 0)
        {
            float finalDamage = stats.FireDamage;
            CalculateCrit(ref finalDamage, stats.CritChance, stats.CritMultiplier);

            DamageInfo info = new DamageInfo
            {
                Amount = stats.FireDamage,
                Type = DamageType.Physical,
                Attacker = stats.gameObject,
                IsCritical = stats.FireDamage != finalDamage,
                KnockBackForce = stats.KnockBackForce
            };

            list.Add(info);
        }

        return list;
    }

    private static float CalculateCrit(ref float baseDamage, float chance, float multiplier) => Random.value * 100f < chance ? baseDamage * multiplier : baseDamage;
}
