using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandler : MonoBehaviour
{
    [SerializeField] private Collider2D hitCollider;
    [SerializeField] private LayerMask hitLayerMask;
    private bool wasEfficiencyTaken = false;
    private bool wasTargetEfficiencyTaken = false;

    //Add weapons system(change weaponController to weapon base class or interface)
    private WeaponController ownerWeapon;
    private void Awake()
    {
        ownerWeapon = GetComponentInParent<WeaponController>();
    }
    private void OnEnable()
    {
        wasEfficiencyTaken = false;
        wasTargetEfficiencyTaken = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitLayerMask.Contains(collision.gameObject.layer) && !wasEfficiencyTaken)
        {
            Debug.Log("Hit");
            wasEfficiencyTaken = true;
            collision.gameObject.GetComponent<IHitable>()?.PlaySFX();
            ownerWeapon.GetComponentInParent<IMove>().TakeEfficiency(ownerWeapon.LastSwingPoint, 10f);

            if (collision.TryGetComponent<IDamageable>(out var target))
            {
                List<DamageInfo> infos = DamageBuilder.BuildForPlayer(ownerWeapon.Stats);
                if (infos != null && infos.Count > 0)
                {
                    foreach (DamageInfo info in infos)
                    {
                        if (!wasTargetEfficiencyTaken)
                        {
                            DamageInfo tempInfo = info;
                            tempInfo.HitPoint = ownerWeapon.LastSwingPoint;
                            target.TakeDamage(tempInfo);
                            wasTargetEfficiencyTaken = true;
                        }
                        else
                            target.TakeDamage(info);
                    }
                }
            }
        }
    }
}
