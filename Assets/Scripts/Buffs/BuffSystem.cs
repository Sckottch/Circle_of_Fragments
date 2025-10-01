using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    List<Unit> activeUnits = new List<Unit>();

    private void OnEnable()
    {
        // Setup event listeners
        WaveManager.OnWaveStarted += HandleWaveStart;
        CombatSetup.OnEnemyUnitAdded += HandleEnemyUnitAdded;

        // Buff event listeners
        TurnManager.OnTurnStarted += HandleTurnStart;
        TurnManager.OnTurnEnded += HandleTurnEnd;

        // Cleanup event listeners
        WaveManager.OnWaveEnded += HandleWaveEnd;
        GameManager.OnCombatEnd += HandleCombatEnd;
    }

    private void OnDisable()
    {
        WaveManager.OnWaveStarted -= HandleWaveStart;
        WaveManager.OnWaveEnded -= HandleWaveEnd;
        CombatSetup.OnEnemyUnitAdded -= HandleEnemyUnitAdded;
        GameManager.OnCombatEnd -= HandleCombatEnd;
        TurnManager.OnTurnStarted -= HandleTurnStart;
        TurnManager.OnTurnEnded -= HandleTurnEnd;
    }

    #region Units Setup

    private void HandleWaveStart(List<PlayableUnit> playerUnits, List<EnemyUnit> enemyUnits)
    {
        activeUnits.Clear();
        activeUnits.AddRange(playerUnits);
        activeUnits.AddRange(enemyUnits);

        foreach(Unit unit in activeUnits)
        {
            unit.OnDeath += HandleUnitDeath;
            unit.OnBuffApplied += HandleBuffApplied;
            unit.OnBuffRemoved += HandleBuffRemoved;
        }

        Debug.Log($"BuffSystem started with {activeUnits.Count} active units.");
    }

    private void HandleEnemyUnitAdded(EnemyUnit enemyUnit)
    {
        if (!activeUnits.Contains(enemyUnit))
        {
            activeUnits.Add(enemyUnit);
            enemyUnit.OnDeath += HandleUnitDeath;
            enemyUnit.OnBuffApplied += HandleBuffApplied;
            enemyUnit.OnBuffRemoved += HandleBuffRemoved;
        }

        Debug.Log($"BuffSystem: Enemy unit added: {enemyUnit.UnitName}. Total active units: {activeUnits.Count}");
    }

    private void HandleUnitDeath(Unit unit)
    {
        activeUnits.Remove(unit);

        unit.OnDeath -= HandleUnitDeath;
        unit.OnBuffApplied -= HandleBuffApplied;
        unit.OnBuffRemoved -= HandleBuffRemoved;
    }

    #endregion

    #region Buff Handlers

    private void HandleBuffApplied(Unit target, Buff buff, Unit caster)
    {
        Buff existingBuff = target.ActiveBuffs.Find(b => b.buffName == buff.buffName);

        if (existingBuff == null)
        {
            Buff newBuff = ScriptableObject.Instantiate(buff);
            target.ActiveBuffs.Add(newBuff);
            newBuff.OnApply(target, caster);

            Debug.Log($"Buff '{newBuff.buffName}' applied to {target.UnitName} by {caster.UnitName}.");
        }
        else
        {
            existingBuff.OnApply(target, caster);
        }

        
    }

    private void HandleBuffRemoved(Unit target, Buff buff)
    {
        Buff toRemoveBuff = target.ActiveBuffs.Find(b => b.buffName == buff.buffName);

        target.ActiveBuffs.Remove(toRemoveBuff);

        toRemoveBuff.OnRemove(target);

        Destroy(toRemoveBuff);
    }

    private void HandleTurnStart(Unit unit)
    {
        List<Buff> buffs = unit.ActiveBuffs.FindAll(b => b.actionTime == BuffActionTime.OnTurnStart && b.duration > 0);

        foreach (Buff buff in buffs)
        {
            buff.ApplyEffect(unit);
        }
    }

    private void HandleTurnEnd(Unit unit)
    {
        ApplyTurnEndBuffs(unit);

        TickBuffs(unit);
    }

    private void ApplyTurnEndBuffs(Unit unit)
    {
        List<Buff> buffs = unit.ActiveBuffs.FindAll(b => b.actionTime == BuffActionTime.OnTurnEnd && b.duration > 0);

        foreach (Buff buff in buffs)
        {
            buff.ApplyEffect(unit);
        }
    }

    private void TickBuffs(Unit unit)
    {
        foreach (Buff buff in unit.ActiveBuffs)
        {
            buff.Tick();

            if (buff.duration <= 0)
            {
                buff.OnRemove(unit);
            }
        }

        unit.ActiveBuffs.RemoveAll(b => b.duration <= 0);
    }

    #endregion

    #region Cleanup

    private void HandleWaveEnd()
    {
        Clear();
    }

    private void HandleCombatEnd(CombatEndResult endResult)
    {
        Clear();
    }

    private void Clear()
    {
        foreach (Unit unit in activeUnits)
        {
            unit.OnDeath -= HandleUnitDeath;
            unit.OnBuffApplied -= HandleBuffApplied;
            unit.OnBuffRemoved -= HandleBuffRemoved;
        }

        activeUnits.Clear();
    }

    #endregion
}
