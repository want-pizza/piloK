using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();
    private int currentWave = 0;

    private void Start()
    {
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        while (currentWave < waves.Count)
        {
            Wave wave = waves[currentWave];

            yield return new WaitForSeconds(wave.delayBeforeStart);

            Debug.Log("RunWaves()");

            foreach (var spawnInfo in wave.spawns)
            {
                StartCoroutine(SpawnWithWarning(spawnInfo));
            }

            currentWave++;
        }
    }

    private IEnumerator SpawnWithWarning(WaveSpawnData spawnInfo)
    {
        Debug.Log("SpawnWithWarning");
        // 1) Створили попередження
        GameObject warning = WarningController.Instance.ShowWarning(
            spawnInfo.enemyType,
            spawnInfo.spawnPoint.position
        );
        Debug.Log("okspodkfsodf");
        // 2) Чекаємо 1 секунду
        yield return new WaitForSeconds(1f);

        // 3) Спавнимо ворога
        EnemyPool.Instance.Get(
            spawnInfo.enemyType,
            spawnInfo.spawnPoint.position
        );

        // 4) Прибираємо попередження
        Destroy(warning);
    }
}
