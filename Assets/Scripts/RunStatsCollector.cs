using UnityEngine;

public class RunStatsCollector : MonoBehaviour
{
    public static RunStatsCollector Instance;

    private RunStats stats;

    private void Awake()
    {
        Instance = this;
    }

    public RunStats GetSnapshot()
    {
        return stats;
    }

    public void BeginRun()
    {
        stats = new RunStats
        {
            maxHP = 100
        };
    }

    public void TryAddMaxHp(int value)
    {
        stats.maxHP = stats.maxHP > value ? stats.maxHP : value;
    }

    public void AddDamageDealt(float value)
    {
        stats.damageDealt += value;
        stats.oneShotMaxDamageDealt = Mathf.Max(stats.oneShotMaxDamageDealt, Mathf.RoundToInt(value));
    }

    public void AddKill()
    {
        stats.enemiesKilled++;
    }

    public void AddCoins(int amount)
    {
        stats.coinsGained += amount;
    }
    public void SetWave(int value)
    {
        stats.wave = value;
    }
    public void SetLevel(int value)
    {
        stats.level = value;
    }
    public void SetSurvivedTime(float value)
    {
        stats.survivedTime += value;
    }
}
