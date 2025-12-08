using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleMageDamageable : Damageable
{
    [Header("SFX clips")]
    [SerializeField] private List<AudioClip> hitClips;
    [SerializeField] private AudioClip deathClip;

    private IMove littleMageMovement;
    private void Start()
    {
        littleMageMovement = GetComponentInParent<IMove>();
        resistances[DamageType.Fire] = 0.7f;

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

        AudioManager.Instance.PlaySFX(hitClips[Random.Range(0, hitClips.Count)], transform.position, 0.7f);
        //FXManager.Instance.Play(hitKey, transform.position, Quaternion.identity);

        Debug.Log($"info.KnockBackForce = {info.KnockBackForce}");

        if (info.HitPoint != null)
        {
            Debug.Log("LittleMage info.HitPoint != null");
            littleMageMovement.TakeEfficiency(info.HitPoint, info.KnockBackForce);
        }
        // запусти партикли в залежності від Type
        // ParticleManager.Play("slime_splat", info.HitPoint);
        // SoundManager.Play("slime_hurt");
    }

    protected override void Die(DamageInfo info)
    {
        Debug.Log("Slime is dead");
        AudioManager.Instance.PlaySFX(deathClip, transform.position);
        //FXManager.Instance.Play(deathKey, transform.position, Quaternion.identity);
        base.Die(info);
    }
}
