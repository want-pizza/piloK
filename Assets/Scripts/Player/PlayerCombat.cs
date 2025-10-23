using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AttackEventChannel attackEventChannel;
    [SerializeField] private WeaponController weaponController;

    [SerializeField] private CharacterFacing facing;
    private PlayerAction.PlayerActions playerActions;

    private Field<bool> isAttacking = new Field<bool>(false);
    private Field<string> currentAttackAnim = new Field<string>("");

    public Field<bool> IsAttacking => isAttacking;
    public Field<string> CurrentAttackAnim => currentAttackAnim;

    [Header("Settings")]
    [SerializeField] private float comboResetTime = 0.5f;
    [SerializeField] private float attackCooldown = 0.25f;

    private int comboStep = 0;
    private float lastAttackTime;
    private bool isCooldown = false;

    private void Awake()
    {
        playerActions = InputManager.Instance.PlayerActions.Player;
    }

    private void OnEnable()
    {
        playerActions.Attack.performed += OnAttackPerformed;
        EventBus.Subscribe("AttackCooldownEnd", OnCooldownEnd);
        attackEventChannel.OnAttackEnded += OnAttackEnded;
    }

    private void OnDisable()
    {
        playerActions.Attack.performed -= OnAttackPerformed;
        EventBus.Unsubscribe("AttackCooldownEnd", OnCooldownEnd);
        attackEventChannel.OnAttackEnded -= OnAttackEnded;
    }

    private void OnAttackEnded()
    {
        isAttacking.Value = false;
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (!CanAttack()) return;

        Debug.Log("StartAttack");
        Vector2 moveInput = playerActions.Move.ReadValue<Vector2>();
        Vector2 attackDir = moveInput.sqrMagnitude > 0.01f
            ? moveInput.normalized
            : (facing.IsFacingRight ? Vector2.right : Vector2.left);

        StartAttack(attackDir);
    }

    private bool CanAttack() 
    {
        return !isAttacking && !isCooldown;
    }

    private void StartAttack(Vector2 dir)
    {
        if (Time.time - lastAttackTime > comboResetTime)
            comboStep = 1;
        else
            comboStep = Mathf.Min(comboStep + 1, 2);

        lastAttackTime = Time.time;

        string animName = $"SwordSwing{comboStep}";
        currentAttackAnim.Value = animName;
        isAttacking.Value = true;

        isCooldown = true;
        TimerManager.Instance.AddTimer(attackCooldown, "AttackCooldownEnd");
    }

    private void OnCooldownEnd()
    {
        isCooldown = false;
    }
}
