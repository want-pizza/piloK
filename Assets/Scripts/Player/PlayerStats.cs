using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Stat<float> MaxHealth = new(100f);
    public Field<float> CurrentHealth = new(100f);
    public Stat<float> ResistTime = new(10f);
    public Stat<float> KnockBackForce = new(2f);
    public Stat<float> SelfKnockBackForceMultiplier = new(2f);
    public Stat<float> CritChance = new(10f);
    public Stat<float> CritMultiplier = new(2f);

    public Stat<float> PhisicalDamage = new(10f);
    public Stat<float> FireDamage = new(0f);
}
