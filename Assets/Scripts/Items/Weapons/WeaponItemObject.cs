using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class WeaponItemObject : ItemStatData
{
    public UnityEngine.GameObject weaponBehaviorPrefab;
    public string[] attackAnimations;
}
