using System;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    [Header("Base Stats")]
    public float maxHP = 100;
    public float currentHP = 100;
    public int xp = 30;

    protected bool isAlive = true;
    private ICharacterLevel lastCharacterLevel;

    [Header("PoolMemberReference")]
    [SerializeField] private EnemyPoolMember enemyPoolMember;

    [Tooltip("Resistance per damage type (0..1 meaning % reduction)")]
    public Dictionary<DamageType, float> resistances = new Dictionary<DamageType, float>()
    {
        { DamageType.Physical, 0f },
        { DamageType.Fire, 0f },
        { DamageType.Ice, 0f },
        { DamageType.Poison, 0f }
    };

    public event Action<DamageInfo, DamageResult> OnDamagedEvent;
    public event Action OnDeathEvent;

    protected virtual void Awake()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }
    protected virtual void OnEnable()
    {
        isAlive = true;
    }

    public virtual DamageResult TakeDamage(DamageInfo info)
    {
        Debug.Log("TakeDamage");
        DamageResult result = CalculateAndApplyDamage(info);
        FloatingTextSpawner.Instance.Spawn(result.FinalAmount.ToString("0.##"), transform.position, info.IsCritical, false);

        OnDamagedEvent?.Invoke(info, result);

        if (result.IsFatal)
            Die(info);

        return result;
    }

    protected virtual DamageResult CalculateAndApplyDamage(DamageInfo info)
    {
        ICharacterLevel characterLevel = info.Attacker.GetComponent<ICharacterLevel>();
        lastCharacterLevel = characterLevel == null ? lastCharacterLevel : characterLevel;

        Debug.Log("CalculateAndApplyDamage");
        float baseAmount = Mathf.Max(0f, info.Amount);
        float currentAmount = baseAmount, resist = 0;

        if (info.Type != DamageType.Void)
        {
            if (info.Type != DamageType.Physical)
            {

                if (resistances.TryGetValue(info.Type, out resist))
                {
                    currentAmount = Mathf.Max(1f, baseAmount * (-resist / 30f + 1));
                }
            }
            currentAmount = info.Type == DamageType.Physical ? currentAmount * Mathf.Max(1f, (-resist / 30f + 1)) : Mathf.Max(1f, currentAmount * (-resist / 50f + 1));
        }

        float absorbed = baseAmount - currentAmount;

        currentHP -= currentAmount;
        bool isDead = currentHP <= 0;

        DamageResult res = new DamageResult
        {
            FinalAmount = currentAmount,
            Absorbed = absorbed,
            Type = info.Type,
            IsFatal = isDead
        };

        return res;
    }

    protected virtual void Die(DamageInfo info)
    {
        if (!isAlive)
            return;

        if (lastCharacterLevel != null)
        {
            // RunStatsCollector
            RunStatsCollector.Instance.AddKill();

            lastCharacterLevel.GainXP(xp);
        }

        isAlive = false;

        OnDeathEvent?.Invoke();

        enemyPoolMember?.Die();
    }
}
