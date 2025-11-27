using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public event Action<StatType, float> OnStatChanged;

    public Stat<float> MaxHealth = new(100f);
    public Field<float> CurrentHealth = new(100f);
    public Stat<float> Damage = new(10f);
    public Stat<float> ResistTime = new(10f);

    private void OnEnable()
    {
        MaxHealth.OnValueChanged += OnMaxHealthChanged;
        CurrentHealth.OnValueChanged += OnCurrentHealthChanged;
        Damage.OnValueChanged += OnDamageChanged;
        ResistTime.OnValueChanged += OnResistTimeChanged;
    }

    private void OnDisable()
    {
        MaxHealth.OnValueChanged -= OnMaxHealthChanged;
        CurrentHealth.OnValueChanged -= OnCurrentHealthChanged;
        Damage.OnValueChanged -= OnDamageChanged;
        ResistTime.OnValueChanged -= OnResistTimeChanged;
    }

    private void OnMaxHealthChanged(float value) =>
        RaiseStatChanged(StatType.MaxHealth, value);

    private void OnCurrentHealthChanged(float value)
    {
        RaiseStatChanged(StatType.CurrentHealth, value);
    }

    private void OnDamageChanged(float value) =>
        RaiseStatChanged(StatType.Damage, value);

    private void OnResistTimeChanged(float value) =>
        RaiseStatChanged(StatType.ResistTime, value);

    private void RaiseStatChanged(StatType type, float value)
    {
        OnStatChanged?.Invoke(type, value);
    }
}
