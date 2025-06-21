using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Stat<float> MaxHealth = new();
    public Stat<float> Damage = new();

    private void OnEnable()
    {
        MaxHealth.OnValueChanged += MaxHealth_OnValueChanged;
        Damage.OnValueChanged += Damage_OnValueChanged;
    }

    private void Damage_OnValueChanged(float obj)
    {
        Debug.Log($"Damage - {obj}");
    }

    private void MaxHealth_OnValueChanged(float obj)
    {
        Debug.Log($"MaxHealth - {obj}");
    }

    public void ShowStats()
    {

    }
    private void OnDisable()
    {
        MaxHealth.OnValueChanged -= MaxHealth_OnValueChanged;
        Damage.OnValueChanged -= Damage_OnValueChanged;
    }
}
