using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolMember : MonoBehaviour
{
    public int enemyType;

    public void Die()
    {
        EnemyPool.Instance.Return(enemyType, gameObject);
    }
}
