using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private DamageType damageType = DamageType.Physical;

    [Tooltip("Who can this object harm?")]
    [SerializeField] private LayerMask targetLayers;

    [Tooltip("Whether to deal damage continuously while the object is inside the trigger")]
    [SerializeField] private bool continuous = false;

    [Tooltip("Time between repeated hits ( if continuous = true )")]
    [SerializeField] private float hitInterval = 1f;

    private float nextHitTime;

    private void OnTriggerEnter2D(Collider2D other) => TryDealDamage(other);

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!continuous) return;

        if (Time.time >= nextHitTime)
        {
            TryDealDamage(other);
            nextHitTime = Time.time + hitInterval;
        }
    }

    private void TryDealDamage(Collider2D other)
    {
        bool layerAllowed = (targetLayers & (1 << other.gameObject.layer)) != 0;

        if (!layerAllowed)
        {
            Debug.Log($"Layer not allowed: {LayerMask.LayerToName(other.gameObject.layer)} on {other.name}");
            return;
        }

        var comps = other.GetComponents<MonoBehaviour>();
        foreach (var c in comps)
        {
            Debug.Log($"Component: {c.GetType().FullName}");
        }

        IDamageable target = other.gameObject.GetComponent<IDamageable>();
        Debug.Log($"target = {target.ToString()}");
        if (target == null) return;

        Debug.Log("bruh");
        DamageInfo info = new DamageInfo
        {
            Amount = baseDamage,
            Type = damageType,
            Attacker = gameObject,
            HitPoint = other.ClosestPoint(transform.position),
            IsCritical = false
        };

        target.TakeDamage(info);
    }
}
