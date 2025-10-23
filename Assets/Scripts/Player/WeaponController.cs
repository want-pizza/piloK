using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponController : MonoBehaviour
{
    [SerializeField] private AttackEventChannel attackChannel;

    private void OnEnable()
    {
        attackChannel.OnSwingStart += OnSwingStartReceived;
        attackChannel.OnSwingEnd += DestroyHitBox;
    }

    private void OnDisable()
    {
        attackChannel.OnSwingStart -= OnSwingStartReceived;
        attackChannel.OnSwingEnd -= DestroyHitBox;
    }

    [Header("Hitbox")]
    [SerializeField] private GameObject hitboxPrefab;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private float hitboxDistance = 0.8f;
    [SerializeField] private float hitboxLifetime = 0.15f;

    private Animator animator;
    private Vector2 lastDir = Vector2.right;

    private GameObject hitbox;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnSwingStartReceived(string name)
    {
        animator.Play(name);
        SpawnHitbox();
    }
    public void SpawnHitbox()
    {
        Vector3 pos = attackOrigin.position + (Vector3)lastDir * hitboxDistance;
        float angle = Mathf.Atan2(lastDir.y, lastDir.x) * Mathf.Rad2Deg;
        hitbox = Instantiate(hitboxPrefab, pos, Quaternion.Euler(0f, 0f, angle));
    }
    public void DestroyHitBox()
    {
        Destroy(hitbox);
    }
}
