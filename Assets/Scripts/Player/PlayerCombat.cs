using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour, ICanBePaused
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
    [SerializeField] private float attackCooldown = 0.3f;

    private int comboStep = 1;
    private float lastAttackTime;
    private bool isCooldown = false;

    private bool isPaused = false;

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
        return !isAttacking && !isCooldown && !isPaused;
    }

    private void StartAttack(Vector2 dir)
    {
        string animName = $"SwordSwing{CalculateCombo()}";
        currentAttackAnim.Value = animName;
        isAttacking.Value = true;

        isCooldown = true;
        TimerManager.Instance.AddTimer(attackCooldown, "AttackCooldownEnd");
    }

    private void OnCooldownEnd()
    {
        lastAttackTime = Time.time;

        isCooldown = false;
        
        //need fix
        //isAttacking.Value = false;
    }
    private int CalculateCombo()
    {
        if (Time.time - lastAttackTime > comboResetTime)
            return comboStep = 1;
        else
            return comboStep = comboStep % 2 + 1;
    }

    public void OnPausedChanged(bool paused)
    {
        isPaused = paused;
    }
}
