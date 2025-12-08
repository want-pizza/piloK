using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private int initialCount = 5;

    private Dictionary<int, Queue<GameObject>> pools = new Dictionary<int, Queue<GameObject>>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            pools[i] = new Queue<GameObject>();

            for (int j = 0; j < initialCount; j++)
            {
                GameObject obj = Instantiate(enemyPrefabs[i]);
                obj.SetActive(false);
                pools[i].Enqueue(obj);
            }
        }
    }

    public GameObject Get(int enemyType, Vector3 position)
    {
        GameObject obj;

        if (pools[enemyType].Count > 0)
            obj = pools[enemyType].Dequeue();
        else
            obj = Instantiate(enemyPrefabs[enemyType]);

        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }

    public void Return(int enemyType, GameObject obj)
    {
        obj.SetActive(false);
        pools[enemyType].Enqueue(obj);
    }
}
