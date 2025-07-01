using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
    public WeaponItemObject weaponData;

    public abstract void Attack(Vector2 lookDirection);
    public abstract void Equip(GameObject player);
    public abstract void Unequip(GameObject player);
}
