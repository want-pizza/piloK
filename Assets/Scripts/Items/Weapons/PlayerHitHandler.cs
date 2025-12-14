using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitHandler : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;

    [Header("Settings")]
    [SerializeField] private Collider2D hitCollider;
    [SerializeField] private LayerMask hitLayerMask;
    private bool wasEfficiencyTaken = false;

    //Add weapons system(change weaponController to weapon base class or interface)
    private WeaponController ownerWeapon;
    private void Awake()
    {
        ownerWeapon = GetComponentInParent<WeaponController>();
    }
    private void OnEnable()
    {
        wasEfficiencyTaken = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitLayerMask.Contains(collision.gameObject.layer))
        {
            Debug.Log("Hit");
            
            collision.gameObject.GetComponent<IHitable>()?.PlaySFX();

            if (!wasEfficiencyTaken)
            {
                ownerWeapon.GetComponentInParent<IMove>().TakeEfficiency(ownerWeapon.LastSwingPoint, 10f);
                wasEfficiencyTaken = true;
            }

            if (collision.TryGetComponent<IDamageable>(out var target))
            {
                List<DamageInfo> infos = DamageBuilder.BuildForPlayer(ownerWeapon.Stats);
                if (infos != null && infos.Count > 0)
                {
                    foreach (DamageInfo info in infos)
                    {
                        DamageInfo tempInfo = info;
                        tempInfo.HitPoint = ownerWeapon.LastSwingPoint;
                        DamageResult result = target.TakeDamage(tempInfo);

                        if (stats.CurrentHealth < stats.MaxHealth)
                        {
                            if (info.Type == DamageType.Physical && Random.Range(0, 100) < stats.VampirismChance)
                            {
                                stats.CurrentHealth.Value += Mathf.Max(1, result.FinalAmount * stats.VampirismStrength / 100);
                                stats.CurrentHealth.Value = Mathf.Min(stats.MaxHealth, stats.CurrentHealth);
                            }
                        }
                    }
                }
            }
        }
    }
}
