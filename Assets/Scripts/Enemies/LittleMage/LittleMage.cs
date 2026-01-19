using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour, IMove
{
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private CharacterFacing characterFacing;
    [SerializeField] private Damageable damageable;

    [Header("Checkers")]
    [SerializeField] private TriggerChecker groundChecker;
    [SerializeField] private TriggerChecker edgeChecker;
    [SerializeField] private TriggerChecker obstacleChecker;

    [Header("MovementSettings")]
    [SerializeField] private float keepDistance = 4f;
    [SerializeField] private float retreatDistance = 2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float efficiencyTime = 0.2f;
    [SerializeField] private LayerMask groundMask;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 16f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("HitSettings")]
    [SerializeField] float KnockbackMultiplier = 2f;

    private bool isGrounded;
    private bool isObstacleAhead;
    private bool isEdgeAhead;
    private bool isRetreatDistance = false;
    private bool isKeepDisatance;

    private float stateTimer;

    private bool isEfficiency = false;

    private bool isAttacking = false;
    private bool isAttackCooldown = false;

    private bool isPaused = false;
    private Coroutine stunCorutine;

    public MageState state = MageState.Idle;
    public MageState State => state;

    private float currentVelocityX;
    float targetVelocityX;
    public float XVelocity => currentVelocityX == 0 ? rb.velocity.x : currentVelocityX * moveSpeed;
    public float YVelocity => rb.velocity.y;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        player = FindAnyObjectByType<PlayerMovement>().transform;
    }

    private void OnEnable()
    {
        state = MageState.Idle;
        groundChecker.OnTriggeredStateChanged += OnGroundedTriggered;
        edgeChecker.OnTriggeredStateChanged += OnEdgeTriggered;
        obstacleChecker.OnTriggeredStateChanged += OnObstacleTriggered;
        damageable.OnDamagedEvent += OnHit;
        damageable.OnDeathEvent += Die;
    }

    private void OnDisable()
    {
        anim.SetBool("isAttacking", false);
        isEfficiency = false;
        isAttackCooldown = false;
        groundChecker.OnTriggeredStateChanged -= OnGroundedTriggered;
        edgeChecker.OnTriggeredStateChanged -= OnEdgeTriggered;
        obstacleChecker.OnTriggeredStateChanged -= OnObstacleTriggered;
        damageable.OnDamagedEvent -= OnHit;
        damageable.OnDeathEvent -= Die;
    }

    private void OnEdgeTriggered(bool value)
    {
        isEdgeAhead = !value;
        TryJumpIfNeeded();
    }

    private void OnObstacleTriggered(bool value)
    {
        isObstacleAhead = value;
        TryJumpIfNeeded();
    }

    private void OnGroundedTriggered(bool value)
    {
        isGrounded = value;
        TryJumpIfNeeded();
    }

    void FixedUpdate()
    {
        if (isPaused)
            return;

        currentVelocityX = Mathf.MoveTowards(
            currentVelocityX,
            targetVelocityX,
            acceleration * Time.fixedDeltaTime
        );

        if(!isEfficiency)
            rb.velocity = new Vector2(currentVelocityX, rb.velocity.y);

        switch (state)
        {
            case MageState.Idle: IdleLogic(); break;
            case MageState.HoldDistance: HoldLogic(); break;
            case MageState.Attacking: AttackLogic(); break;
            case MageState.Cooldown: CooldownLogic(); break;
            case MageState.Stunned: break;
            case MageState.Dead: break;
        }

        UpdateAnimations();
    }

    // ────────────────────── LOGIC ──────────────────────

    void IdleLogic()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (!anim.GetBool("isAttacking"))
        {
            state = MageState.HoldDistance;
            return;
        }
    }

    void HoldLogic()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        int dir = GetPlayerDirection();
        bool isClearLineToPlayer = ClearLineToPlayer();

        // --- 1) Тримання дистанції ---
        if (dist < retreatDistance)
        {
            Debug.Log("isRetreatDistance");
            isRetreatDistance = true;
            Move(-dir);
            return;
        }
        else
        {
            isRetreatDistance = false;
        }

        // --- 2) Нема прямої лінії, але ми вміщаємось в рамки дистанції ---
        if (!isClearLineToPlayer && (characterFacing.IsFacingRight == (GetPlayerDirection() == 1)))
        {
            // якщо є прірва попереду — стоїмо
            if (isEdgeAhead)
            {
                Debug.Log("isEdgeAhead");
                Move(0);
                return;
            }

            // якщо ми далеко — підходимо ближче
            if (dist > keepDistance)
            {
                Debug.Log("keepDistance");
                Move(dir);
                return;
            }

            // якщо ми в нормальній дистанції але нема видимості — підходимо доки не буде видно
            Move(dir);
            Debug.Log("seekPlayer");
            return;
        }

        // --- 3) Є пряма лінія, нема кулдауна атаки ---
        if (isClearLineToPlayer && !isAttackCooldown && dist > retreatDistance)
        {
            // Поворот на гравця перед атакою
            if (characterFacing.IsFacingRight ? dir == -1 : dir == 1)
                Move(dir);
            Debug.Log("attack");
            isAttacking = true;
            state = MageState.Attacking;
            return;
        }

        // --- 4) Якщо все ок просто підтримуємо дистанцію ---
        if (dist > keepDistance)
        {
            Debug.Log("keepDistance4");
            isKeepDisatance = false;
            Move(dir);
        }
        else
        {
            Debug.Log("keepDistance0");
            isKeepDisatance = true;
            Move(0);
        }
            
    }

    void AttackLogic()
    {
        if (!(characterFacing.IsFacingRight == (GetPlayerDirection() == 1)))
            currentVelocityX = GetPlayerDirection() / 100f;

        anim.SetBool("isAttacking", true);

        stateTimer = attackCooldown;
        AttackCoolDown(attackCooldown);
        state = MageState.Idle;
    }

    void CooldownLogic()
    {
        Move(0);

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            state = MageState.HoldDistance;
        }
    }

    // ──────────────────── HELPERS ────────────────────-
    void Move(int dir)
    {
        if (isEfficiency || isObstacleAhead || (isEdgeAhead && !isRetreatDistance))
        {
            bool conditionEfficiency = isEfficiency;
            bool conditionObstacle = isObstacleAhead;
            bool conditionEdgeRetreat = isEdgeAhead && !isRetreatDistance;
            bool conditionKeepDistance = !isKeepDisatance;


            Debug.Log(
        $"AI Decision triggered:\n" +
        $"- isEfficiency: {isEfficiency}\n" +
        $"- isObstacleAhead: {isObstacleAhead}\n" +
        $"- isEdgeAhead && !isRetreatDistance: {conditionEdgeRetreat}\n" +
        $"- !isKeepDisatance: {conditionKeepDistance}"
    );
            return;
        }
        
        //Debug.Log($"mOVE dir -{dir}");
        float targetVelocityX = isGrounded ? dir * moveSpeed: dir * moveSpeed/2;

        // Рухаємо по осі X
        rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);
    }

    int GetPlayerDirection()
    {
        return transform.position.x < player.position.x ? 1 : -1;
    }

    void TryJumpIfNeeded()
    {
        if (!isGrounded)
            return;

        Debug.Log($"isEdgeAhead = {isEdgeAhead}; isObstacleAhead = {isObstacleAhead};");
        if ((isEdgeAhead && isRetreatDistance) || isObstacleAhead)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    bool ClearLineToPlayer()
    {
        Vector2 dir = (player.position - attackPoint.position).normalized;
        float dist = Vector2.Distance(attackPoint.position, player.position);

        if (dist > attackRange)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(attackPoint.position, dir, dist, obstacleMask);

        return hit.collider == null;
    }


    // ───────────────── Анімація ────────────────────
    void UpdateAnimations()
    {
        anim.SetFloat("VelocityY", YVelocity);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isRunning", Mathf.Abs(rb.velocity.x) > 0.1f);
    }

    // ───────────────── ПОРАЖЕННЯ / STUN ─────────────────
    public void AttackCoolDown(float time)
    {
        isAttackCooldown = true;
        stunCorutine = StartCoroutine(StunRoutine(time));
    }

    IEnumerator StunRoutine(float t)
    {
        yield return new WaitForSeconds(t);
        isAttackCooldown = false;
    }

    public void Die()
    {
        state = MageState.Dead;
        rb.velocity = Vector2.zero;
    }
    private void OnHit(DamageInfo info, DamageResult result)
    {
        anim.Play("Hit");
    }

    public void TakeEfficiency(Vector2 point, float power)
    {
        //Debug.Log("SlimeTakeEfficiency");
        if (isEfficiency)
            return;
        //Debug.Log($"SlimeTakeEfficiency point = ({point.x}; {point.y}); power = {power}");

        rb.AddForce(new Vector2(power * 0.7f * point.x * KnockbackMultiplier, power * 0.3f * KnockbackMultiplier), ForceMode2D.Impulse);
        isEfficiency = true;
        StartCoroutine(TurnOffEfficiency(efficiencyTime));
    }

    IEnumerator TurnOffEfficiency(float time)
    {
        yield return new WaitForSeconds(time);
        isEfficiency = false;
    }

    public void OnPausedChanged(bool paused)
    {
        isPaused = paused;
    }
}


public enum MageState
{
    Idle,
    HoldDistance,
    Attacking,
    Jumping,
    Cooldown,
    Stunned,
    Dead
}

