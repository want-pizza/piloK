using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HitHandler : MonoBehaviour
{
    [SerializeField] private Collider2D hitCollider;
    private List<Collision2D> colliders = new List<Collision2D>();
    private bool wasEfficiencyTaken = false;

    //Add weapons system(change weaponController to weapon base class or interface)
    private WeaponController ownerWeapon;

    public void Init(WeaponController weapon)
    {
        ownerWeapon = weapon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && !wasEfficiencyTaken) //spike 
        {
            wasEfficiencyTaken = true;
            collision.gameObject.GetComponent<IHitable>().PlaySFX();
            ownerWeapon.GetComponentInParent<IMove>().TakeEfficiency(ownerWeapon.LastDir, 10f);
        }
        if (collision.TryGetComponent<IDamageable>(out var target))
        {
            //DamageSystem.Instance.ProcessHit(ownerWeapon, target, collision.gameObject);
        }

        Destroy(gameObject); 
    }
}
