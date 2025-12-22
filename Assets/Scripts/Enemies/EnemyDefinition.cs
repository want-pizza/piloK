using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyType Cost", menuName = "Waves/Enemy Definition")]
public class EnemyDefinition : ScriptableObject
{
    public int id;
    public EnemyType type;
    public float basePowerCost;
    public GameObject prefab;
}
