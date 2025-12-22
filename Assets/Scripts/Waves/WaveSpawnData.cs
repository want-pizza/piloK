using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveSpawnData
{
    public EnemyDefinition enemy;
    public Transform spawnPoint;
}

[System.Serializable]
public class Wave
{
    public float delayBeforeStart = 1f;
    public List<WaveSpawnData> spawns = new List<WaveSpawnData>();
}
