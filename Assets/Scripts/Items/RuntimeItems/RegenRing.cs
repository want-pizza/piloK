using UnityEngine;

public class RegenRing : RuntimeItemBehaviour
{
    private const float TICK_INTERVAL = 2f;
    private const float HEAL_AMOUNT = 1f;

    private float timer;

    private void Update()
    {
        if (context == null)
            return;

        timer += Time.deltaTime;

        if (timer < TICK_INTERVAL)
            return;

        timer -= TICK_INTERVAL;
        TryHeal();
    }

    private void TryHeal()
    {
        var stats = context.PlayerStats;

        float current = stats.CurrentHealth.Value;
        float max = stats.MaxHealth.Value;

        if (current >= max)
            return;

        stats.CurrentHealth.Value = Mathf.Min(
            current + HEAL_AMOUNT,
            max
        );
    }
}
