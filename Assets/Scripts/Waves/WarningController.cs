using System.Collections.Generic;
using UnityEngine;

public class WarningController : MonoBehaviour
{
    public static WarningController Instance;
    public List<GameObject> warningPrefabs;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject ShowWarning(int enemyType, Vector3 pos)
    {
        GameObject go = Instantiate(warningPrefabs[enemyType], pos, Quaternion.identity);
        return go;
    }
}
