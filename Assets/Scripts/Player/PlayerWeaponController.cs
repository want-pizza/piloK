using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour, IWeaponController
{
    [Header("One weapon settings")]
    [SerializeField] private string[] attackAnimations;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float damage = 10f;

    private int attackIndex = 0;

    [SerializeField] private TriggerChecker groundChecker;
    [SerializeField] private CharacterFacing characterFacing;

    private PlayerAction inputActions;

    [Header("Maybe will inplement later")]
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
    public string GetNextAttackAnimation()
    {
        if (attackAnimations.Length == 0)
            return "DefaultAttack";

        string anim = attackAnimations[attackIndex];
        attackIndex = (attackIndex + 1) % attackAnimations.Length;
        return anim;
    }
    public float GetAttackSpeed() => attackSpeed;
    public float GetDamage() => damage;
    private bool isGrounded()
    {
        return groundChecker.IsTriggered;
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
    private void OnDisable()
    {
        inputActions.Player.Attack.started -= OnAttackPerformed;
        unequipEventChannel.OnItemEquipped -= UnequipWeapon;
        equipEventChannel.OnItemEquipped -= EquipWeapon;
    }
}
