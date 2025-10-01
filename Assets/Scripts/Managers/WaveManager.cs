using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private Queue<WaveDataSO> waves;
    private WaveDataSO currentWave;

    private List<PlayableUnit> activePlayerUnits = new();
    private List<EnemyUnit> activeEnemyUnits = new();

    private int currentWaveIndex = 0;

    public static event Action<WaveDataSO> OnWaveSetup;
    public static event Action<List<PlayableUnit>, List<EnemyUnit>> OnWaveStarted;
    public static event Action<List<PlayableUnit>, List<EnemyUnit>> OnActiveUnitsUpdated;
    public static event Action OnWaveEnded;

    private void OnEnable()
    {
        GameManager.OnCombatStart += HandleCombatStart;
        CombatSetup.OnPlayerUnitsInitialized += HandlePlayerUnitsInitialized;
        CombatSetup.OnEnemyUnitsInitialized += HandleEnemyUnitsInitialized;
        OnActiveUnitsUpdated += HandleActiveUnitsUpdate;
        OnWaveEnded += StartNextWave;
    }

    private void OnDisable()
    {
        GameManager.OnCombatStart -= HandleCombatStart;
        CombatSetup.OnPlayerUnitsInitialized -= HandlePlayerUnitsInitialized;
        CombatSetup.OnEnemyUnitsInitialized -= HandleEnemyUnitsInitialized;
        OnActiveUnitsUpdated -= HandleActiveUnitsUpdate;
        OnWaveEnded -= StartNextWave;
    }

    private void HandleCombatStart(CombatData data)
    {
        InitializeWavesData(data.waves);
    }

    private void InitializeWavesData(List<WaveDataSO> waves)
    {
        this.waves = new Queue<WaveDataSO>(waves);
    }

    private void HandlePlayerUnitsInitialized(List<PlayableUnit> playerUnits)
    {
        Debug.Log("Player units initialized.");
        activePlayerUnits = playerUnits;

        StartNextWave();
    }

    public static void UpdateActiveUnits(List<PlayableUnit> playerUnits, List<EnemyUnit> enemyUnits)
    {
        OnActiveUnitsUpdated?.Invoke(playerUnits, enemyUnits);
    }

    private void HandleActiveUnitsUpdate(List<PlayableUnit> playerUnits, List<EnemyUnit> enemyUnits)
    {
        activePlayerUnits.Clear();
        activeEnemyUnits.Clear();

        activePlayerUnits = playerUnits;
        activeEnemyUnits = enemyUnits;
    }
    private void StartNextWave()
    {
        if (waves == null || waves.Count == 0)
        {
            Debug.Log("No more waves to start.");

            CombatEndResult result = new();
            result.playerWon = true;

            GameManager.CombatEnd(result);
            return;
        }

        currentWave = waves.Dequeue();

        currentWaveIndex++;
        Debug.Log($"Starting Wave {currentWaveIndex}");
        OnWaveSetup?.Invoke(currentWave);
    }

    private void HandleEnemyUnitsInitialized(List<EnemyUnit> enemyUnits)
    {
        activeEnemyUnits.Clear();
        activeEnemyUnits = enemyUnits;

        //Debug.Log($"Enemy units initialized. Count: {activeEnemyUnits.Count}");
        //Debug.Log($"Player units initialized. Count: {activePlayerUnits.Count}");

        OnWaveStarted?.Invoke(activePlayerUnits, activeEnemyUnits);
    }

    public static void EndWave()
    {
        OnWaveEnded?.Invoke();
    }
}
