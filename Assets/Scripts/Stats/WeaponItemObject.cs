using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class WeaponItemObject : BaseItemObject
{
    private bool isEquipped = false;
    public override bool CanEquip => true;
    public override bool CanUnequip => true;
    public override bool IsEquipped => isEquipped;

    public override void Equip(PlayerStats stats)
    {
        if (CanEquip)
        {
            isEquipped = true;
            Debug.Log($"Equipped weapon {name}");
        }
    }

    public override void Unequip(PlayerStats stats)
    {
        if (CanUnequip)
        {
            isEquipped = false;
            Debug.Log($"Unequipped weapon {name}");
        }
    }

    //public void Attack(Vector2 direction, Transform owner)
    //{
    //    Vector3 spawnPos = owner.position + (Vector3)(direction.normalized * distance);
    //    Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

    //    GameObject hitbox = Instantiate(hitboxPrefab, spawnPos, rotation);
    //    hitbox.GetComponent<Spear>().Init(owner, direction, damage);
    //    Destroy(hitbox, duration);
    //}
}
