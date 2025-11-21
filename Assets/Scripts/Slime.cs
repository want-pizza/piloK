using UnityEngine;

public class Slime : MonoBehaviour, IMove
{
    private SlimeState state = SlimeState.Idle;
    public SlimeState State => state;

    public float XVelocity => rb.velocity.x;

    public float YVelocity => rb.velocity.y;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private CircleCollider2D attackZone;
    [SerializeField] private TriggerChecker groundChecker;

    [Header("Settings")]
    [SerializeField] private float detectRange = 6f;
    [SerializeField] private float patrolRange = 3f;
    [SerializeField] private float patrolMoveDelay = 2f;

    [SerializeField] private float jumpHorizontalSpeed = 3f;
    [SerializeField] private float jumpVerticalSpeed = 5f;
    [SerializeField] private float jumpInterval = 1.2f;

    [SerializeField] private float groundCheckDistance = 0.1f;

    [Header("Patrol Settings")]
    public bool canPatrol = true;
    public float patrolJumpInterval = 1.5f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float edgeCheckDistance = 0.4f;

    private int direction = 1; // 1 = right, -1 = left
    private float patrolTimer;

    private bool isGrounded = false;
    private float jumpTimer;

    private Vector2 patrolOrigin;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>().transform; //need changed
        rb = GetComponentInChildren<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        attackZone = GetComponentInChildren<CircleCollider2D>();
        //groundChecker = GetComponentInChildren<TriggerChecker>();
    }
    private void OnEnable()
    {
        groundChecker.OnTriggeredStateChanged += SetGrounded;
    }
    private void OnDisable()
    {
        groundChecker.OnTriggeredStateChanged -= SetGrounded;
    }

    void Start()
    {
        patrolOrigin = transform.position;
    }

    void Update()
    {
        switch (state)
        {
            case SlimeState.Idle: IdleLogic(); break;
            case SlimeState.Patrol: PatrolLogic(); break;
            case SlimeState.Chasing: ChasingLogic(); break;
            case SlimeState.Jumping: JumpingLogic(); break;
            case SlimeState.Attacking: AttackingLogic(); break;
            case SlimeState.Cooldown: CooldownLogic(); break;
            case SlimeState.Stunned: break;
        }

        UpdateAnimations();
    }

    // ---------------- LOGIC ---------------- //

    void IdleLogic()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < detectRange)
        {
            state = SlimeState.Chasing;
            return;
        }

        patrolTimer -= Time.deltaTime;
        if (patrolTimer <= 0)
        {
            state = SlimeState.Patrol;
            patrolTimer = patrolMoveDelay;
        }
    }

    void PatrolLogic()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist < detectRange)
        {
            state = SlimeState.Chasing;
            return;
        }

        patrolTimer -= Time.deltaTime;

        if (patrolTimer <= 0f)
        {
            if (PredictJumpLandingSafe())
            {
                direction *= -1;
                transform.localScale = new Vector3(direction, 1, 1);
                patrolTimer = 0.5f;
                return;
            }

            TryJumpInDirection(direction);
            patrolTimer = patrolJumpInterval;
        }
    }


    void ChasingLogic()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < attackZone.radius)
        {
            state = SlimeState.Attacking;
            return;
        }

        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0)
        {
            TryJumpTowardPlayer();
            state = SlimeState.Jumping;
            jumpTimer = jumpInterval;
        }
    }

    void JumpingLogic()
    {
        if (isGrounded)
        {
            state = SlimeState.Cooldown;
            jumpTimer = 0.3f;
        }
    }

    void CooldownLogic()
    {
        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0f)
        {
            state = SlimeState.Chasing;
        }
    }

    void AttackingLogic()
    {
        // атака в≥дбудетьс€ через trigger enter
        // тут просто ставимо таймер
        state = SlimeState.Cooldown;
        jumpTimer = 0.5f;
    }

    public void OnSuccessfulHit()
    {
        // ѕри попаданн≥ слайм переходить у cooldown
        state = SlimeState.Cooldown;
        jumpTimer = 0.4f;
    }

    // ---------------- HELPERS ---------------- //

    void TryJumpInDirection(int dir)
    {
        Debug.Log($"jump; isGrounded ={isGrounded}");
        if (isGrounded) 
            rb.AddForce(new Vector2(dir * jumpHorizontalSpeed, jumpVerticalSpeed), ForceMode2D.Impulse);
    }

    void TryJumpTowardPlayer()
    {
        Debug.Log("jump to player");
        if (!isGrounded)
            return;
        Vector2 dir = (player.position - transform.position).normalized;
        rb.AddForce(new Vector2(dir.x * jumpHorizontalSpeed, jumpVerticalSpeed), ForceMode2D.Impulse);
    }

    void UpdateAnimations()
    {
        bool isJumping = rb.velocity.y > 0.1f;
        bool isFalling = rb.velocity.y < -0.1f;

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isFalling", isFalling);

        anim.SetBool("isStunned", state == SlimeState.Stunned);
    }

    bool PredictJumpLandingSafe()
    {
        Vector2 start = groundCheck.position; // або transform.position
        Vector2 velocity = new Vector2(direction * jumpHorizontalSpeed, jumpVerticalSpeed);

        const int steps = 12;     // точн≥сть
        const float timeStep = 0.07f;

        for (int i = 0; i < steps; i++)
        {
            float t = i * timeStep;

            // ‘ормула параболи руху
            Vector2 point = start + velocity * t + 0.5f * Physics2D.gravity * (t * t);

            // DEBUG Ч малюЇмо траЇктор≥ю
            Debug.DrawLine(point, point + Vector2.down * 0.2f, Color.yellow);

            // –ейкаст вниз
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.down, 0.25f, groundMask);

            // якщо земл≥ нема це небезпечний стрибок
            if (hit.collider == null)
                return false;
        }

        return true; // всю траЇктор≥ю тримаЇ над землею стрибаЇмо
    }


    public void TakeEfficiency(Vector2 direction, float power)
    {
        throw new System.NotImplementedException();
    }

    public void OnPausedChanged(bool paused)
    {
        throw new System.NotImplementedException();
    }

    private void SetGrounded(bool value) { Debug.Log($"SetGrounded({value})"); isGrounded = value; }
}

public enum SlimeState
{
    Idle,
    Patrol,
    Chasing,
    Jumping,
    Attacking,
    Cooldown,
    Stunned   // пустий, зробимо п≥зн≥ше
}
