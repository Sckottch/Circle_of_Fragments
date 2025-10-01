using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatSetup : MonoBehaviour
{
    [SerializeField] private Transform playerTeamTransform;
    [SerializeField] private Transform enemyTeamTransform;

    [Space(10)]
    [Header("UI")]
    [SerializeField] private CombatUIManager combatUIManager;

    private int enemyMaxCount = 5;

    private List<PlayerUnitData> playerUnits = new();
    private Queue<EnemySpawnInfo> enemyUnits = new();

    public List<PlayableUnit> ActivePlayerUnits { get; private set; } = new List<PlayableUnit>();
    public List<EnemyUnit> ActiveEnemyUnits { get; private set; } = new List<EnemyUnit>();

    public static event Action<List<PlayableUnit>> OnPlayerUnitsInitialized;
    public static event Action<List<EnemyUnit>> OnEnemyUnitsInitialized;
    public static event Action<EnemyUnit> OnEnemyUnitAdded;

    private void OnEnable()
    {
        GameManager.OnCombatStart += HandleCombatStart;
        WaveManager.OnWaveSetup += HandleWaveSetup;
    }

    private void OnDisable()
    {
        GameManager.OnCombatStart -= HandleCombatStart;
        WaveManager.OnWaveSetup -= HandleWaveSetup;
    }

    private void HandleCombatStart(CombatData data)
    {
        playerUnits = data.playerUnits;

        InitializePlayerUnits();
    }

    private void InitializePlayerUnits()
    {
        Vector3[] offsets =
        {
            new(0,0,0),
            new(-3,3,0),
            new(-3,-3,0),
            new(-6,0,0)
        };

        foreach(GameObject child in playerTeamTransform)
        {
            Destroy(child);
        }

        ActivePlayerUnits.Clear();

        for (int i = 0; i < playerUnits.Count; i++)
        {
            PlayerUnitData unit = playerUnits[i];
            PlayableUnit playerUnit = Instantiate(unit.unitData.unitPrefab, playerTeamTransform);
            playerUnit.transform.position += offsets[i];
            playerUnit.InitializeFromPlayerData(unit);

            ActivePlayerUnits.Add(playerUnit);
        }

        OnPlayerUnitsInitialized?.Invoke(ActivePlayerUnits);
    }

    private void HandleWaveSetup(WaveDataSO waveData)
    { 
        enemyUnits = new Queue<EnemySpawnInfo>(waveData.enemies);

        foreach (GameObject child in enemyTeamTransform)
            Destroy(child);

        ActiveEnemyUnits.Clear();

        SpawnEnemyUnits();

        SetupUI();
    }

    private void SpawnEnemyUnits()
    {
        if(enemyUnits.Count <= 0)
            return;

        while (enemyUnits.Count > 0 && ActiveEnemyUnits.Count < enemyMaxCount)
        {
            EnemySpawnInfo enemyInfo = enemyUnits.Dequeue();
            EnemyUnit enemyUnit = Instantiate(enemyInfo.enemyData.unitPrefab, enemyTeamTransform);
            enemyUnit.transform.position += enemyInfo.offset;

            enemyUnit.Initialize(enemyInfo.enemyData);

            enemyUnit.OnDeath += HandleEnemyDeath;

            ActiveEnemyUnits.Add(enemyUnit);
        }

        OnEnemyUnitsInitialized?.Invoke(ActiveEnemyUnits);
    }

    private void SetupUI()
    {
        combatUIManager.SetupUI(ActivePlayerUnits, ActiveEnemyUnits);
    }

    private void HandleEnemyDeath(Unit unit)
    {
        Debug.Log($"Enemy {unit.UnitName} has died.");

        ActiveEnemyUnits.Remove(unit as EnemyUnit);
        SpawnNextEnemy(unit);
        unit.OnDeath -= HandleEnemyDeath;
        Destroy(unit.gameObject);

        WaveManager.UpdateActiveUnits(ActivePlayerUnits, ActiveEnemyUnits);
    }

    private void SpawnNextEnemy(Unit unit)
    {
        if(enemyUnits.Count <= 0 || ActiveEnemyUnits.Count >= enemyMaxCount)
            return;

        Vector3 offset = unit.transform.localPosition;

        EnemySpawnInfo enemyInfo = enemyUnits.Dequeue();
        EnemyUnit enemyUnit = Instantiate(enemyInfo.enemyData.unitPrefab, enemyTeamTransform);
        enemyUnit.transform.position += offset;

        enemyUnit.Initialize(enemyInfo.enemyData);

        enemyUnit.OnDeath += HandleEnemyDeath;
        ActiveEnemyUnits.Add(enemyUnit);

        combatUIManager.AddUnit(enemyUnit);
        OnEnemyUnitAdded?.Invoke(enemyUnit);
    }
}
