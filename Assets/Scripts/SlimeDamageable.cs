using System.Diagnostics;

public class SlimeDamageable : Damageable
{
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
        Debug.WriteLine("Slime is dead");
        //base.Die(info);
        // наприклад, граємо death animation/розділення на два
    }
}
