using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class WeaponItemObject : ItemStatData
{
    public GameObject weaponBehaviorPrefab;
    public AnimationClip[] attackAnimations;
}
