using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rock : WeaponBehavior
{
    public override void Equip(UnityEngine.GameObject player)
    {
        Debug.Log($"Equipped {weaponData.name}");
    }

    public override void Attack(Vector2 lookDirection)
    {
        Debug.Log($"attack - {weaponData.name}");
    }

    public override void Unequip(UnityEngine.GameObject player)
    {
        Debug.Log($"Unequipped {weaponData.name}");
    }
}
