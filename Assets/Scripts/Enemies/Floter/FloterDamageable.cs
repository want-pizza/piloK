using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloterDamageable : Damageable
{
    [Header("VFX keys")]
    [SerializeField] private string hitKey = "FloterHit";
    [SerializeField] private string deathKey = "FloterDeath";

    [Header("SFX clips")]
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip deathClip;

    private IMove floterMovement;

    protected override void Awake()
    {
        base.Awake();
        floterMovement = GetComponentInParent<IMove>();
    }

    private void OnEnable()
    {
        OnDamagedEvent += OnHit;
        currentHP = maxHP;
        isAlive = true;
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

        if (info.KnockBackDirection != null)
        {
            Debug.Log("Floter info.HitPoint != null");
            floterMovement.TakeEfficiency(info.KnockBackDirection, info.KnockBackForce);
        }
    }

    protected override void Die(DamageInfo info)
    {
        Debug.Log("Floter is dead");
        AudioManager.Instance.PlaySFX(deathClip, transform.position);
        FXManager.Instance.Play(deathKey, transform.position, Quaternion.identity);

        base.Die(info);
    }
}
