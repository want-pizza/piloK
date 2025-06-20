using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Spear")]
public class SpearItem : ScriptableObject, IWeapon
{
    [SerializeField] private GameObject hitboxPrefab;
    [SerializeField] private float distance = 1.2f;
    [SerializeField] private float duration = 0.15f;
    [SerializeField] private float damage = 10f; //  налаштовується через інспектор

    public void Attack(Vector2 direction, Transform owner)
    {
        Vector3 spawnPos = owner.position + (Vector3)(direction.normalized * distance);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

        GameObject hitbox = Instantiate(hitboxPrefab, spawnPos, rotation);
        hitbox.GetComponent<SpearHitbox>().Init(owner, direction, damage);
        Destroy(hitbox, duration);
    }
}
