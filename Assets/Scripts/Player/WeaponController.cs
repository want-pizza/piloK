using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(Animator))]
public class WeaponController : MonoBehaviour
{
    [SerializeField] private AttackEventChannel attackChannel;
    [SerializeField] private CharacterFacing characterFacing;

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
    [SerializeField] private float hitboxDistance = 1.5f;
    [SerializeField] private float hitboxLifetime = 0.15f;

    private Animator animator;
    private Vector2 lastDir = Vector2.right;
    public Vector2 LastDir => lastDir;

    private GameObject hitbox;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnSwingStartReceived(string name)
    {
        if (characterFacing.IsFacingRight)
        {
            transform.rotation = Quaternion.identity;
            lastDir = Vector2.right;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            lastDir = Vector2.left;
        }
        animator.Play(name);
        
        SpawnHitbox(name);
    }
    private void SpawnHitbox(string name)
    {
        Vector3 pos = Vector3.zero;

        if (name == "SwingUp")
        {
            lastDir = Vector2.up;
            pos = attackOrigin.position + Vector3.up * hitboxDistance;
        }
        else if (name == "SwingDown")
        {
            lastDir = Vector2.down;
            pos = attackOrigin.position + Vector3.down * hitboxDistance;
        }
        else
        {
            pos = attackOrigin.position + (Vector3)lastDir * hitboxDistance;

        }
        hitbox = Instantiate(hitboxPrefab, pos, Quaternion.identity);
        hitbox.GetComponent<HitHandler>().Init(this);
    }
    public void DestroyHitBox()
    {
        Destroy(hitbox);
    }
}
