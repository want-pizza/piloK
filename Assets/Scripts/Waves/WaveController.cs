using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private WaveTimerUI waveTimerUI;
    [SerializeField] private WaveScalingSettings scaling;
    [SerializeField] private List<EnemyDefinition> enemies;
    [SerializeField] private List<SpawnPoint> spawnPoints;

    private readonly List<EnemyPoolMember> activeEnemies = new();

    public float interWaveDelay = 2f;

    private int currentWave = 0;
    private float timer;
    private bool isInterWave = false;

    public event System.Action<int, float> OnTimerChanged;
    public event System.Action<int> OnWaveStarted;
    public event System.Action<float> OnInterWave;

    public float CurrentWaveTimeLeft => timer;
    public bool IsInterWave => isInterWave;

    private void Start()
    {
        StartWaveCountdown();
    }

    private void OnEnable()
    {
        EnemyPoolMember.OnEnemyDied += HandleEnemyDied;
        waveTimerUI.Subscribe(this);
    }

    private void OnDisable()
    {
        EnemyPoolMember.OnEnemyDied -= HandleEnemyDied;
        waveTimerUI.Unsubscribe(this);
    }

    private void HandleEnemyDied(EnemyPoolMember enemy)
    {
        activeEnemies.Remove(enemy);
        if (activeEnemies.Count!=0)
            return;

        if (!IsInterWave && timer > 0f && activeEnemies.Count == 0)
        {
            HandleEarlyWaveClear();
        }
    }

    private void HandleEarlyWaveClear()
    {
        GiveEarlyClearReward();

        StartCoroutine(InterWaveRoutine());
    }

    private void GiveEarlyClearReward()
    {
        if (!isInterWave)
        {
            float timeRatio = timer;
            int coins = Mathf.CeilToInt(10 * timeRatio);

            CoinController.Instance.AddCoins(coins);
        }
    }

    private void Update()
    {
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

        yield return null;
    }

    private void StartNextWave()
    {
        isInterWave = false;

        float wavePower = CalculateWavePower(currentWave);
        float waveDuration = CalculateWaveDuration(wavePower);

        timer = waveDuration;
        OnWaveStarted?.Invoke(currentWave);

        var spawns = GenerateWave(wavePower);

        foreach (var spawn in spawns)
            StartCoroutine(SpawnWithWarning(spawn));

        currentWave++;
    }

    private IEnumerator SpawnWithWarning(WaveSpawnData spawnInfo)
    {
        GameObject warning = WarningController.Instance.ShowWarning(
            spawnInfo.enemy.id,
            spawnInfo.spawnPoint.position
        );

        yield return new WaitForSeconds(1f);

        GameObject enemyGO = EnemyPool.Instance.Get(spawnInfo.enemy.id, spawnInfo.spawnPoint.position);

        EnemyPoolMember enemy = enemyGO.GetComponent<EnemyPoolMember>();
        activeEnemies.Add(enemy);

        Destroy(warning);
    }

    private float CalculateWavePower(int waveIndex)
    {
        return scaling.startPower * Mathf.Pow(scaling.powerGrowth, waveIndex);
    }

    private float CalculateWaveDuration(float power)
    {
        return Mathf.Min(
            scaling.baseDuration + power * scaling.durationPerPower,
            scaling.maxDuration
        );
    }

    private List<WaveSpawnData> GenerateWave(float powerBudget)
    {
        List<WaveSpawnData> result = new();

        int minEnemies = Mathf.FloorToInt(powerBudget / 8f);
        int maxEnemies = Mathf.CeilToInt(powerBudget / 5f);

        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
        enemyCount = Mathf.Min(enemyCount, spawnPoints.Count);

        List<SpawnPoint> shuffledPoints = new(spawnPoints);
        Shuffle(shuffledPoints);

        float targetAverageCost = powerBudget / enemyCount;

        int totalCost = 0;
        foreach (SpawnPoint point in shuffledPoints)
        {
            if (powerBudget <= 0f)
                break;

            EnemyDefinition chosenEnemy = ChooseEnemy(point, targetAverageCost, powerBudget);
            if (chosenEnemy == null)
                continue;

            float cost = chosenEnemy.basePowerCost * point.GetCostMultiplier(chosenEnemy.type);

            result.Add(new WaveSpawnData
            {
                enemy = chosenEnemy,
                spawnPoint = point.transform
            });

            powerBudget -= cost;
            totalCost += (int)cost;
        }

        float error = Mathf.Abs(totalCost - powerBudget) / powerBudget;

        return result;
    }

    private EnemyDefinition ChooseEnemy(SpawnPoint point, float targetCost, float remainingBudget)
    {
        EnemyDefinition best = null;
        float bestDelta = float.MaxValue;

        foreach (var enemy in enemies)
        {
            float cost = enemy.basePowerCost * point.GetCostMultiplier(enemy.type);

            if (cost > remainingBudget)
                continue;

            float delta = Mathf.Abs(cost - targetCost);

            if (delta < bestDelta)
            {
                bestDelta = delta;
                best = enemy;
            }
        }

        return best;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}