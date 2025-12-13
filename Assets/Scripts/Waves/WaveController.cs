using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private WaveTimerUI waveTimerUI;
    [SerializeField] private List<Wave> waves = new List<Wave>();
    public float interWaveDelay = 2f;

    private int currentWave = 0;
    private float timer;
    private bool isInterWave = false;

    public event System.Action<int, float> OnTimerChanged;
    public event System.Action<int> OnWaveStarted;
    public event System.Action<float> OnInterWave;

    public float CurrentWaveTimeLeft => timer;
    public bool IsInterWave => isInterWave;

    private void OnEnable()
    {
        waveTimerUI.Subscribe(this);
    }
    private void OnDisable()
    {
        waveTimerUI.Unsubscribe(this);
    }

    private void Start()
    {
        StartWaveCountdown();
    }

    private void Update()
    {
        if (currentWave >= waves.Count)
            return;

        timer -= Time.deltaTime;

        if (!isInterWave)
        {
            OnTimerChanged?.Invoke(currentWave, timer);

            if (timer <= 0f)
            {
                StartCoroutine(InterWaveRoutine());
            }
        }
        else
        {
            OnInterWave?.Invoke(timer);

            if (timer <= 0f)
            {
                StartNextWave();
            }
        }
    }

    private void StartWaveCountdown()
    {
        isInterWave = true;
        timer = interWaveDelay;
    }

    private IEnumerator InterWaveRoutine()
    {
        isInterWave = true;
        timer = interWaveDelay;

        //тут можна грати анімації переходу
        yield return null;
    }

    private void StartNextWave()
    {
        isInterWave = false;

        if (currentWave >= waves.Count)
            return;

        Wave wave = waves[currentWave];
        timer = wave.delayBeforeStart;

        OnWaveStarted?.Invoke(currentWave);

        foreach (var spawnInfo in wave.spawns)
            StartCoroutine(SpawnWithWarning(spawnInfo));

        currentWave++;
    }

    private IEnumerator SpawnWithWarning(WaveSpawnData spawnInfo)
    {
        GameObject warning = WarningController.Instance.ShowWarning(
            spawnInfo.enemyType,
            spawnInfo.spawnPoint.position
        );

        yield return new WaitForSeconds(1f);

        EnemyPool.Instance.Get(
            spawnInfo.enemyType,
            spawnInfo.spawnPoint.position
        );

        Destroy(warning);
    }
}
