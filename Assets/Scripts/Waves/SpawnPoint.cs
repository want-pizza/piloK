using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPointPowerModifier
{
    public EnemyType enemyType;
    public float powerMultiplier = 1f;
}

public class SpawnPoint : MonoBehaviour
{
    public List<SpawnPointPowerModifier> modifiers;

    public float GetCostMultiplier(EnemyType type)
    {
        var mod = modifiers.Find(m => m.enemyType == type);
        return mod != null ? mod.powerMultiplier : 99999f;
    }
}

public enum EnemyType
{
    GreenSlime,
    LittleMage,
    Floter
}