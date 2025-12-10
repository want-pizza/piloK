using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarController : MonoBehaviour
{
    [SerializeField] PlayerStats stats;
    [SerializeField] HealthBar healthBar;
    
    private float maxHealth, currentHealth;
    private float lasthealth = 100;
    private float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            OnHealthChanged();
        }
    }
    private float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            OnHealthChanged();
        }

    }

    private void OnEnable()
    {
        currentHealth = stats.CurrentHealth.Value;
        maxHealth = stats.MaxHealth.Value;

        stats.CurrentHealth.OnValueChanged += OnCurrentHealthChanged;
        stats.MaxHealth.OnValueChanged += OnMaxHealthChanged;
    }
    private void OnDisable()
    {
        stats.CurrentHealth.OnValueChanged -= OnCurrentHealthChanged;
        stats.MaxHealth.OnValueChanged -= OnMaxHealthChanged;
    }
    private void OnCurrentHealthChanged(float value)
    {
        CurrentHealth = value;
    }

    private void OnMaxHealthChanged(float value)
    {
        MaxHealth = value;
    }

    private void OnHealthChanged()
    {
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if(lasthealth < currentHealth)
        {
            FloatingTextSpawner.Instance.Spawn($"{currentHealth - lasthealth}", transform.position, false, true);
        }
        lasthealth = currentHealth;
    }
}
