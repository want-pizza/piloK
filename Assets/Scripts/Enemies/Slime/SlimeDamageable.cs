using UnityEngine;
using Unity.VisualScripting;

public class SlimeDamageable : Damageable
{
    [Header("VFX keys")] 
    [SerializeField] private string hitKey = "GreenSlimeHit";
    [SerializeField] private string deathKey = "GreenSlimeDeath";

    [Header("SFX clips")]
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip deathClip;

    private IMove slimeMovement;
    private void Start()
    {
        slimeMovement = GetComponentInParent<IMove>();
        // свої резисти: наприклад слиз вразливий до fire
        resistances[DamageType.Fire] = -0.2f; // -20% -> означає +20% damage (можна інтерпретувати)
        
    }
    private void OnEnable()
    {
        OnDamagedEvent += OnHit;
        currentHP = maxHP;
    }
    private void OnDisable()
    {
        OnDamagedEvent -= OnHit;
    }

    private void OnHit(DamageInfo info, DamageResult result)
    {
        AudioManager.Instance.PlaySFX(hitClip, transform.position);
        FXManager.Instance.Play(hitKey, transform.position, Quaternion.identity);

        Debug.Log($"info.KnockBackForce = {info.KnockBackForce}");

        if (info.HitPoint != null)
        {
            Debug.Log("Slime info.HitPoint != null");
            slimeMovement.TakeEfficiency(info.HitPoint, info.KnockBackForce);
        }
        // запусти партикли в залежності від Type
        // ParticleManager.Play("slime_splat", info.HitPoint);
        // SoundManager.Play("slime_hurt");
    }

    protected override void Die(DamageInfo info)
    {
        Debug.Log("Slime is dead");
        AudioManager.Instance.PlaySFX(deathClip, transform.position);
        FXManager.Instance.Play(deathKey, transform.position, Quaternion.identity);

        base.Die(info);
    }
}
