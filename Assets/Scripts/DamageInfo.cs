using UnityEngine;

public enum DamageType
{
    Physical,
    Fire,
    Ice,
    Poison,
}

public struct DamageInfo
{
    public int Amount;
    public DamageType Type;
    public GameObject Attacker;
    public Vector2 HitPoint;
    public bool IsCritical;
}

public struct DamageResult
{
    public int FinalAmount;
    public int Absorbed;
    public DamageType Type;
    public bool IsFatal;
}
