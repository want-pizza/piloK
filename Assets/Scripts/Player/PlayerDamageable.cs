using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    [Header("VFX keys")]
    [SerializeField] private string hitKey = "PlayerHit";
    [SerializeField] private string deathKey = "PlayerDeath";

    [Header("SFX clips")]
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip deathClip;

    private IMove playerMovement;
    private PlayerStats playerStats;
    private Collider2D damageableCollider;
    private void Awake()
    {
        playerMovement = GetComponentInParent<IMove>();
        playerStats = GetComponentInParent<PlayerStats>();
        damageableCollider = GetComponent<Collider2D>();
    }
    private void OnEnable()
    {
        OnDamagedEvent += OnHit;
    }
    private void OnDisable()
    {
        OnDamagedEvent -= OnHit;

    }

    protected override DamageResult CalculateAndApplyDamage(DamageInfo info)
    {
        currentHP = playerStats.CurrentHealth;
        return base.CalculateAndApplyDamage(info);
    }

    private void OnHit(DamageInfo info, DamageResult result)
    {
        AudioManager.Instance.PlaySFX(hitClip);
        //FXManager.Instance.Play(hitKey, transform.position, Quaternion.identity);

        //Debug.Log($"info.KnockBackForce = {info.KnockBackForce}");
        Debug.Log($"Player take {result.FinalAmount} damage");

        if(result.FinalAmount < 999f)
            playerMovement.TakeEfficiency(info.HitPoint, info.KnockBackForce);

        StartCoroutine(ResistFrames(playerStats.ResistTime));

        playerStats.CurrentHealth.Value = currentHP;
    }
    private IEnumerator ResistFrames(float time)
    {
        Debug.Log("damageableCollider.enabled = false;");
        damageableCollider.enabled = false;

        yield return new WaitForSeconds(time);
        damageableCollider.enabled = true;
        Debug.Log("damageableCollider.enabled = true;");
    }

    protected override void Die(DamageInfo info)
    {
        AudioManager.Instance.PlaySFX(deathClip);
    }
}
