using System;
using UnityEngine;

[Flags]
public enum DamageType
{
    Void,
    Physical,
    Fire,
    Ice,
    Poison
}

public struct DamageInfo
{
    public float Amount;
    public DamageType Type;
    public UnityEngine.GameObject Attacker;
    public Vector2 KnockBackDirection;
    public bool IsCritical;
    public float KnockBackForce;
}

public struct DamageResult
{
    public float FinalAmount;
    public float Absorbed;
    public DamageType Type;
    public bool IsFatal;
}
