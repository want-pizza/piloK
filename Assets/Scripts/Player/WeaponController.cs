using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponController : MonoBehaviour
{
    [Header("Hitbox")]
    [SerializeField] private GameObject hitboxPrefab;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private float hitboxDistance = 0.8f;
    [SerializeField] private float hitboxLifetime = 0.15f;

    private Animator animator;
    private Vector2 lastDir = Vector2.right;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TryAttack(Vector2 dir, int comboStep)
    {
        lastDir = dir;
        string trigger = DirectionToAnim(dir, comboStep);
        //animator.SetTrigger(trigger);
    }

    // Animation Event
    public void SpawnHitbox()
    {
        Vector3 pos = attackOrigin.position + (Vector3)lastDir * hitboxDistance;
        float angle = Mathf.Atan2(lastDir.y, lastDir.x) * Mathf.Rad2Deg;
        var hitbox = Instantiate(hitboxPrefab, pos, Quaternion.Euler(0f, 0f, angle));
        Destroy(hitbox, hitboxLifetime);
    }

    private string DirectionToAnim(Vector2 dir, int comboStep)
    {
        string comboSuffix = comboStep > 1 ? "2" : "1";

        if (Vector2.Dot(dir, Vector2.up) > 0.7f) return $"Attack_Up";
        if (Vector2.Dot(dir, Vector2.down) > 0.7f) return $"Attack_Down";
        if (Vector2.Dot(dir, Vector2.right) > 0.7f) return $"Attack_Right{comboSuffix}";
        if (Vector2.Dot(dir, Vector2.left) > 0.7f) return $"Attack_Left{comboSuffix}";
        return $"Attack_Right{comboSuffix}";
    }
}
