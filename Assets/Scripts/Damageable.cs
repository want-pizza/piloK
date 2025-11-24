using System;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    [Header("Base Stats")]
    public int maxHP = 100;
    public int currentHP = 100;

    [Header("Defenses")]
    [Tooltip("Simple armor value for Physical")]
    public float armor = 10f; // числова броня -> впливає на physical
    [Tooltip("Resistance per damage type (0..1 meaning % reduction)")]
    public Dictionary<DamageType, float> resistances = new Dictionary<DamageType, float>()
    {
        { DamageType.Physical, 0f },
        { DamageType.Fire, 0f },
        { DamageType.Ice, 0f },
        { DamageType.Poison, 0f }
    };

    // Подія, яку можна підписати: гра звуків/частинок/UI
    public event Action<DamageInfo, DamageResult> OnDamagedEvent;
    public event Action OnDeathEvent;

    protected virtual void Awake()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    // --- Основний публічний метод для виклику ззовні ---
    public virtual DamageResult TakeDamage(DamageInfo info)
    {
        Debug.Log("TakeDamage");
        DamageResult result = CalculateAndApplyDamage(info);
        // викликаємо подію для звуків/партиклів/логіки
        OnDamagedEvent?.Invoke(info, result);

        if (result.IsFatal)
            Die(info);

        return result;
    }

    // --- Розрахунок: можна перевизначити в підкласі ---
    protected virtual DamageResult CalculateAndApplyDamage(DamageInfo info)
    {
        Debug.Log("CalculateAndApplyDamage");
        int baseAmount = Mathf.Max(0, info.Amount);

        // 1) базова редукція від типу (резисти як 0..1)
        float typeResist = 0f;
        if (resistances != null && resistances.TryGetValue(info.Type, out float r))
            typeResist = Mathf.Clamp01(r);

        float afterType = baseAmount * (1f - typeResist);

        // 2) броня (physical) - приклад формули: зменшення за формулою armor/(armor + 100)
        float armorReduction = 0f;
        if (info.Type == DamageType.Physical)
        {
            armorReduction = armor / (armor + 100f); // нормалізуємо
        }

        float afterArmor = afterType * (1f - armorReduction);

        int final = Mathf.Max(0, Mathf.RoundToInt(afterArmor));
        int absorbed = baseAmount - final;

        // застосувати
        currentHP -= final;
        currentHP = 0;
        bool isDead = currentHP <= 0;

        DamageResult res = new DamageResult
        {
            FinalAmount = final,
            Absorbed = absorbed,
            Type = info.Type,
            IsFatal = isDead
        };

        return res;
    }

    protected virtual void Die(DamageInfo info)
    {
        OnDeathEvent?.Invoke();
        // за замовчуванням — просто вимикаємо об'єкт (переопреділяй в класах)
        transform.parent.gameObject.SetActive(false);
    }
}
