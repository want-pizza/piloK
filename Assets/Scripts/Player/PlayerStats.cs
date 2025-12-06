using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class PlayerStats : MonoBehaviour
{
    public Stat<float> MaxHealth = new(100f);
    public Field<float> CurrentHealth = new(100f);
    public Stat<float> ResistTime = new(0.2f);
    public Stat<float> KnockBackForce = new(4f);
    public Stat<float> SelfKnockBackForceMultiplier = new(1f);
    public Stat<float> CritChance = new(10f);
    public Stat<float> CritMultiplier = new(2f);

    public Stat<float> PhisicalDamage = new(10f);
    public Stat<float> FireDamage = new(0f);

    private Dictionary<StatType, Stat<float>> dictionary = new Dictionary<StatType, Stat<float>>();
    public Dictionary<StatType, Stat<float>> GetAllStats() => dictionary;

    public Stat<float> GetStatByType(StatType type) => dictionary[type];
    private void Awake()
    {
        BuildDictionary();
    }
    private void BuildDictionary()
    {
        dictionary.Clear();

        var fields = typeof(PlayerStats).GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            if (field.FieldType == typeof(Stat<float>))
            {
                Stat<float> stat = field.GetValue(this) as Stat<float>;

                if (stat != null)
                {
                    if (Enum.TryParse(field.Name, out StatType type))
                    {
                        dictionary.Add(type, stat);
                    }
                    else
                    {
                        Debug.LogWarning($"StatType does not contain '{field.Name}'");
                    }
                }
            }
        }
    }
    private void OnEnable()
    {
        PhisicalDamage.OnValueChanged += value => Debug.Log($"phisicalDamage = {value}"); 
    }
}
