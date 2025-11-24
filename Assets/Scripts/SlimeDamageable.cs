using UnityEngine;
using Unity.VisualScripting;

public class SlimeDamageable : Damageable
{
    [SerializeField] private string deathKey = "Death";
    private void Start()
    {
        // свої резисти: наприклад слиз вразливий до fire
        resistances[DamageType.Fire] = -0.2f; // -20% -> означає +20% damage (можна інтерпретувати)
        OnDamagedEvent += OnHit;
    }

    private void OnHit(DamageInfo info, DamageResult result)
    {
        // запусти партикли в залежності від Type
        // ParticleManager.Play("slime_splat", info.HitPoint);
        // SoundManager.Play("slime_hurt");
    }

    protected override void Die(DamageInfo info)
    {
        Debug.Log("Slime is dead");
        FXManager.Instance.Play(deathKey, transform.position, Quaternion.identity);

        base.Die(info);
    }
}
