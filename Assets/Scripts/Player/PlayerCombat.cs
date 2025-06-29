using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private WeaponItemObject spear;
    [SerializeField] private TriggerChecker groundChecker;
    [SerializeField] private CharacterFacing characterFacing;

    private PlayerAction inputActions;

    private void Awake()
    {
        inputActions = InputManager.Instance.PlayerActions;
    }

    private void OnEnable()
    {
        inputActions.Player.Attack.started += ctx => TryAttack();
    }

    private void TryAttack()
    {
        Vector2 attackDir = GetAttackDirection();
        //spear.Attack(attackDir, attackOrigin);
    }

    private Vector2 GetAttackDirection()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        Vector2 baseDir = characterFacing.IsFacingRight == true ? Vector2.right: Vector2.left;

        if (moveInput.y > 0.5f)
            return Vector2.up;
        if (moveInput.y < -0.5f && !isGrounded())
            return Vector2.down;

        return baseDir;
    }

    private bool isGrounded()
    {
        return groundChecker.IsTriggered;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.started -= ctx => TryAttack();
    }
}
