using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour, IWeaponController
{
    [SerializeField] private TriggerChecker groundChecker;
    [SerializeField] private CharacterFacing characterFacing;

    private PlayerAction inputActions;

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private InventoryEquipEventChannelSO equipEventChannel;
    [SerializeField] private InventoryEquipEventChannelSO unequipEventChannel;
    private WeaponBehavior currentWeaponBehavior;

    private void Awake()
    {
        inputActions = InputManager.Instance.PlayerActions;
    }

    private void OnEnable()
    {
        equipEventChannel.OnItemEquipped += EquipWeapon;
        unequipEventChannel.OnItemEquipped += UnequipWeapon;
        inputActions.Player.Attack.started += OnAttackPerformed;
    }
    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        //if(InputManager.Instance.CurrentState == PlayerState.Normal)
            if (currentWeaponBehavior != null)
            {
                Vector2 lookDirection = GetAttackDirection();
                currentWeaponBehavior.Attack(lookDirection);
            }
    }

    public void EquipWeapon(BaseItemObject weaponItem)
    {
        Debug.Log($"weaponItem equiped = {weaponItem.name}");

        GameObject spawnedWeapon = Instantiate(
            ((WeaponItemObject)weaponItem).weaponBehaviorPrefab,
            weaponHolder.position,
            weaponHolder.rotation,
            weaponHolder
        );

        currentWeaponBehavior = spawnedWeapon.GetComponent<WeaponBehavior>();
        Debug.Log($"currentWeaponBehavior.name - {currentWeaponBehavior.name}");
    }

    private void UnequipWeapon(BaseItemObject weaponItem)
    {
        Debug.Log($"weaponItem unequiped = {weaponItem.name}");
        Destroy(currentWeaponBehavior.gameObject);
        //can add sound, play animation itp
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
        unequipEventChannel.OnItemEquipped -= UnequipWeapon;
        equipEventChannel.OnItemEquipped -= EquipWeapon;
    }
}
