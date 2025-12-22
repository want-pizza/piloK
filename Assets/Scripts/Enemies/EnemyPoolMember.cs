using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolMember : MonoBehaviour
{
    public int enemyType;

    public static event Action<EnemyPoolMember> OnEnemyDied;

    public void Die()
    {
        OnEnemyDied?.Invoke(this);
        EnemyPool.Instance.Return(enemyType, gameObject);
    }
}
