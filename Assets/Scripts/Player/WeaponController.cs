using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterFacing facing;
    [SerializeField] private AttackEventChannel attackEventChannel;
    [SerializeField] private PlayerStats stats;

    [Header("Hitboxes")]
    [SerializeField] private GameObject hitboxRight;
    [SerializeField] private GameObject hitboxUp;
    [SerializeField] private GameObject hitboxDown;

    private GameObject currentHitbox;

    private Animator animator;
    public Vector2 LastSwingPoint { get; private set; }
    public PlayerStats Stats => stats;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if(stats == null)
            stats = GetComponentInParent<PlayerStats>();

        DisableAll();
    }
    private void OnEnable()
    {
        attackEventChannel.OnSwingStart += StartAttack;
        attackEventChannel.OnHitboxOn += EnableHitbox;
        attackEventChannel.OnHitboxOff += DisableHitbox;
    }
    private void OnDisable()
    {
        attackEventChannel.OnSwingStart -= StartAttack;
        attackEventChannel.OnHitboxOn -= EnableHitbox;
        attackEventChannel.OnHitboxOff -= DisableHitbox;
    }

    public void StartAttack(string triggerName)
    {
        animator.Play(triggerName);
    }

    // Animation Event
    public void EnableHitbox(string direction)
    {
        Debug.Log($"direction - {direction}");
        DisableAll();

        switch (direction)
        {
            case "Right":
                
                LastSwingPoint = facing.IsFacingRight ? Vector2.right : Vector2.left;
                currentHitbox = hitboxRight;
                break;

            case "Right1":
                LastSwingPoint = facing.IsFacingRight ? Vector2.right : Vector2.left;
                currentHitbox = hitboxRight;
                break;

            case "Up":
                LastSwingPoint = Vector2.up;
                currentHitbox = hitboxUp;
                break;

            case "Down":
                LastSwingPoint = Vector2.down;
                currentHitbox = hitboxDown;
                break;

            default:
                Debug.Log("Incorrect direction");
                break;
        }

        transform.rotation = facing.IsFacingRight ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);
        currentHitbox.SetActive(true);
    }

    // Animation Event
    public void DisableHitbox()
    {
        DisableAll();
    }

    private void DisableAll()
    {
        hitboxRight?.SetActive(false);
        hitboxUp?.SetActive(false);
        hitboxDown?.SetActive(false);
    }
}
