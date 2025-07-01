using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour, IWeaponController
{
    [SerializeField] private TriggerChecker groundChecker;
    [SerializeField] private CharacterFacing characterFacing;

    private PlayerAction inputActions;

    [SerializeField] private Transform weaponHolder;
    private WeaponBehavior currentWeaponBehavior;

    private void Awake()
    {
        inputActions = InputManager.Instance.PlayerActions;
    }

    private void OnEnable()
    {
        inputActions.Player.Attack.started += OnAttackPerformed;
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (currentWeaponBehavior != null)
        {
            Vector2 lookDirection = GetAttackDirection();
            currentWeaponBehavior.Attack(lookDirection);
        }
    }

    public void EquipWeapon(WeaponItemObject weaponItem)
    {
        if (currentWeaponBehavior != null)
        {
            Destroy(currentWeaponBehavior.gameObject);
        }

        GameObject spawnedWeapon = Instantiate(
            weaponItem.weaponBehaviorPrefab,
            weaponHolder.position,
            weaponHolder.rotation,
            weaponHolder
        );

        currentWeaponBehavior = spawnedWeapon.GetComponent<WeaponBehavior>();
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
        inputActions.Player.Attack.started -= OnAttackPerformed;
    }
}
