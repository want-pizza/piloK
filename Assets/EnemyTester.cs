using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTester : MonoBehaviour
{
    [SerializeField] private int enemyId;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            EnemyPool.Instance.Get(enemyId, new Vector3(0, -1, 0));
    }
}
