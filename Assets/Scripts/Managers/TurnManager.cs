using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private TurnOrderUI turnOrderUI;

    private List<PlayableUnit> playerUnits = new();
    private List<EnemyUnit> enemyUnits = new();
    private List<UnitTurnData> activeUnits = new();


    //Events
    public static event Action<Unit> OnTurnStarted;
    public static event Action<Unit> OnTurnEnded;

    private void OnEnable()
    {
        GameManager.OnCombatEnd += HandleCombatEnd;
        WaveManager.OnWaveStarted += HandleWaveStarted;
        WaveManager.OnWaveEnded += HandleWaveEnd;
        CombatSetup.OnEnemyUnitAdded += HandleEnemyUnitAdded;
        OnTurnEnded += HandleTurnEnd;
    }

    private void OnDisable()
    {
        GameManager.OnCombatEnd -= HandleCombatEnd;
        WaveManager.OnWaveStarted -= HandleWaveStarted;
        WaveManager.OnWaveEnded -= HandleWaveEnd;
        CombatSetup.OnEnemyUnitAdded -= HandleEnemyUnitAdded;
        OnTurnEnded -= HandleTurnEnd;
    }

    private void HandleWaveStarted(List<PlayableUnit> playerUnits, List<EnemyUnit> enemyUnits)
    {
        Initialize(playerUnits, enemyUnits);

        SetupInitialUnits();

        StartNextTurn();
    }

    private void Initialize(List<PlayableUnit> playerUnits, List<EnemyUnit> enemyUnits)
    {
        this.playerUnits.Clear();
        this.enemyUnits.Clear();

        this.playerUnits.AddRange(playerUnits);
        this.enemyUnits.AddRange(enemyUnits);

        Debug.Log($"Initializing TurnManager with {playerUnits.Count} player units and {enemyUnits.Count} enemy units.");

        activeUnits.Clear();
        activeUnits.AddRange(playerUnits.Select(unit => new UnitTurnData(unit)));
        activeUnits.AddRange(enemyUnits.Select(unit => new UnitTurnData(unit)));

        UpdateUI();
    }

    #region Unit Setup

    private void SetupInitialUnits()
    {
        foreach (UnitTurnData unitData in activeUnits)
        {
            Unit unit = unitData.Unit;

            unit.OnAVModify += HandleAVModify;
            unit.OnSpeedChanged += HandleSpeedChange;

            if (unit.IsPlayer)
            {
                unit.OnDeath += HandlePlayerUnitDeath;
            }
            else
            {
                unit.OnDeath += HandleEnemyUnitDeath; 
            }

        }
    }

    private void HandleEnemyUnitAdded(EnemyUnit enemyUnit)
    {
        if (enemyUnit == null) return;

        Debug.Log($"Adding new enemy unit: {enemyUnit.name}");

        UnitTurnData newUnitData = new(enemyUnit);
        activeUnits.Add(newUnitData);
        enemyUnits.Add(enemyUnit);

        enemyUnit.OnDeath += HandleEnemyUnitDeath;
        enemyUnit.OnAVModify += HandleAVModify;
        enemyUnit.OnSpeedChanged += HandleSpeedChange;
        UpdateUI();
    }

    private void HandlePlayerUnitDeath(Unit unit)
    {
        activeUnits.RemoveAll(u => u.Unit == unit);

        UpdateUI();

        unit.OnDeath -= HandlePlayerUnitDeath;
        unit.OnAVModify -= HandleAVModify;
        unit.OnSpeedChanged -= HandleSpeedChange;

        if (playerUnits.Count(u => u.IsAlive()) == 0)
        {
            CombatEndResult result = new();
            result.playerWon = false;

            GameManager.CombatEnd(result);
        }
    }

    private void HandleEnemyUnitDeath(Unit unit)
    {
        activeUnits.RemoveAll(u => u.Unit == unit);
        EnemyUnit enemy = unit as EnemyUnit;
        if (enemy != null)
        {
            enemyUnits.Remove(enemy);
        }

        UpdateUI();

        unit.OnDeath -= HandleEnemyUnitDeath;
        unit.OnAVModify -= HandleAVModify;
        unit.OnSpeedChanged -= HandleSpeedChange;

        if (enemyUnits.Count() == 0)
        {
            WaveManager.EndWave();
        }
    }

    private void HandleAVModify(Unit unit, float value)
    {
        UnitTurnData unitData = activeUnits.FirstOrDefault(u => u.Unit == unit);

        if (unitData != null)
        {
            unitData.ApplyAVModifier(value);
            UpdateUI();
        }
    }

    private void HandleSpeedChange(Unit unit)
    {
        UnitTurnData unitData = activeUnits.FirstOrDefault(u => u.Unit == unit);

        if (unitData != null)
        {
            unitData.SpeedChanged();
            UpdateUI();
        }
    }

    #endregion

    #region Turn Management

    private void StartNextTurn()
    {
        if (activeUnits.Count == 0) return;

        if (activeUnits.Count(u => u.CurrentActionValue <= 0) == 0)
        {
            Tick();
        }

        UnitTurnData nextUnitData = activeUnits.FirstOrDefault(u => u.CurrentActionValue <= 0);
        Unit nextUnit = nextUnitData.Unit;

        if (!nextUnit.CanAct())
        {
            Debug.Log($"Unit {nextUnit.UnitName} cannot act this turn. Skipping to next unit.");

            nextUnitData.RecalculateActionValue();
            UpdateUI();

            StartNextTurn();
            return;
        }

        Debug.Log($"Starting turn for unit: {nextUnit.UnitName}");

        nextUnitData.RecalculateActionValue();

        UpdateUI();
        OnTurnStarted?.Invoke(nextUnit);
    }

    private void Tick()
    {
        float minActionValue = activeUnits.Min(u => u.CurrentActionValue);

        foreach (UnitTurnData data in activeUnits)
        {
            data.ReduceActionValue(minActionValue);
        }
    }

    private void HandleTurnEnd(Unit unit)
    { 
        unit.SetMaterial(GameManager.Instance.gameResources.defaultMaterial);

        StartNextTurn();
    }

    public static void EndTurn(Unit unit)
    {
        OnTurnEnded?.Invoke(unit);
    }

    #endregion

    #region UI

    private void UpdateUI()
    {
        turnOrderUI.SetupIcons(activeUnits.OrderBy(u => u.CurrentActionValue).ToList());
    }

    #endregion

    #region Cleanup

    private void HandleWaveEnd()
    {
        Clear();
    }

    private void HandleCombatEnd(CombatEndResult result)
    {
        Clear();
    }

    private void Clear()
    {
        foreach (UnitTurnData unitData in activeUnits)
        {
            Unit unit = unitData.Unit;

            unit.OnAVModify -= HandleAVModify;
            unit.OnSpeedChanged -= HandleSpeedChange;

            if (unit.IsPlayer)
            {
                unit.OnDeath -= HandlePlayerUnitDeath;
            }
            else
            {
                unit.OnDeath -= HandleEnemyUnitDeath;
            }
        }

        activeUnits.Clear();
        playerUnits.Clear();
        enemyUnits.Clear();

        turnOrderUI.Clear();
    }

    #endregion
}
