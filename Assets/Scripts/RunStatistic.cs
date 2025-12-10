using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStatistic : MonoBehaviour
{
    public static RunStatistic Instance;

    public int maxHP;
    public int level;
    public float survivedTime;
    public float damageDealt;
    public float damageTaken;

    private void Awake()
    {
        if (Instance != this)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ResetStats()
    {
        maxHP = 0;
        level = 0;
        survivedTime = 0f;
        damageDealt = 0f;
        damageTaken = 0f;
    }
}
