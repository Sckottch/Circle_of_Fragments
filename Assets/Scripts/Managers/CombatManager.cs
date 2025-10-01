using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private ActionPanel actionPanel;

    private Unit currentUnit;

    private List<Unit> activeUnits = new();

    private SkillDataSO currentSkill;

    private bool isWaitingPlayerAction = false;
    public static bool IsWaitingTarget { get; private set; } = false;

    private Action<Unit> onTargetSelected;

    private void OnEnable()
    {
        GameManager.OnCombatEnd += HandleCombatEnd;
        CombatSetup.OnEnemyUnitAdded += HandleEnemyUnitAdded;
        WaveManager.OnWaveStarted += HandleWaveStarted;
        WaveManager.OnWaveEnded += HandleWaveEnd;
        TurnManager.OnTurnStarted += HandleTurnStart;
    }

    private void OnDisable()
    {
        GameManager.OnCombatEnd -= HandleCombatEnd;
        CombatSetup.OnEnemyUnitAdded -= HandleEnemyUnitAdded;
        WaveManager.OnWaveStarted -= HandleWaveStarted;
        WaveManager.OnWaveEnded -= HandleWaveEnd;
        TurnManager.OnTurnStarted -= HandleTurnStart;
    }

    #region Combat Setup

    private void HandleWaveStarted(List<PlayableUnit> playerUnits, List<EnemyUnit> enemyUnits)
    {
        activeUnits.Clear();
        activeUnits.AddRange(playerUnits);
        activeUnits.AddRange(enemyUnits);

        SetupUnits();
    }

    private void SetupUnits()
    {
        foreach (Unit unit in activeUnits)
        {
            unit.OnDeath += HandleUnitDeath;
            unit.OnUnitSelected += HandleUnitSelected;
        }
    }

    private void HandleEnemyUnitAdded(EnemyUnit enemyUnit)
    {
        activeUnits.Add(enemyUnit);
        enemyUnit.OnDeath += HandleUnitDeath;
        enemyUnit.OnUnitSelected += HandleUnitSelected;
    }

    private void HandleUnitDeath(Unit unit)
    {
        activeUnits.Remove(unit);

        unit.OnDeath -= HandleUnitDeath;
        unit.OnUnitSelected -= HandleUnitSelected;
    }

    private void HandleUnitSelected(Unit unit)
    {
        if (!IsWaitingTarget) return;

        if (TargetSystem.IsValidTarget(unit, currentUnit, activeUnits, currentSkill))
        {
            ResetTargetsOutline();

            onTargetSelected?.Invoke(unit);
            IsWaitingTarget = false;
            return;
        }
        else
        {
            Debug.LogWarning($"Invalid target selected: {unit.UnitName} for skill: {currentSkill.skillName}");
            // visual feedback for invalid target can be added here
        }
    }

    private void ResetTargetsOutline()
    {
        List<Unit> targets = TargetSystem.GetValidTargets(currentUnit, activeUnits, currentSkill);

        foreach (Unit target in targets)
        {
            target.SetMaterial(GameManager.Instance.gameResources.defaultMaterial);
        }
    }

    #endregion

    #region Combat Actions

    private void HandleTurnStart(Unit unit)
    {
        currentUnit = unit;

        SetUnitOutline();

        StartCoroutine(TurnAction());
    }

    private void SetUnitOutline()
    {
        Material material = GameManager.Instance.gameResources.playerOutlineMaterial;
        if (material == null) return;

        currentUnit.SetMaterial(material);
    }

    private IEnumerator TurnAction()
    {
        yield return new WaitForSeconds(0.5f);

        if(currentUnit.IsPlayer)
        {
            isWaitingPlayerAction = true;

            ShowActionPanel(currentUnit);

            yield return new WaitUntil(() => !isWaitingPlayerAction);
        }
        else
        {
            Unit target = activeUnits
                .Where(unit => unit.IsPlayer)
                .OrderBy(_ => RandomManager.RangeFloat(0, 1))
                .FirstOrDefault();

            currentUnit.TurnAction(target, activeUnits, ActionType.BasicAttack);
        }

        yield return new WaitForSeconds(0.5f);

        TurnManager.EndTurn(currentUnit);

    }

    public void ShowActionPanel(Unit unit)
    {
        actionPanel.Open(unit, PlayerActionSignal);
    }

    public void PlayerActionSignal(Unit unit, ActionType actionType)
    {
        PlayableUnit playableUnit = unit as PlayableUnit;

        switch (actionType)
        {
            case ActionType.BasicAttack:
                currentSkill = playableUnit.BasicSkill.SkillData;
                break;

            case ActionType.Skill:
                currentSkill = playableUnit.SpecialSkill.SkillData;
                break;

            case ActionType.Ultimate:
                currentSkill = playableUnit.UltimateSkill.SkillData;
                break;
        }

        if (currentSkill.isSelf)
        {
            unit.TurnAction(unit, activeUnits, actionType);
            isWaitingPlayerAction = false;
            return;
        }

        StartTargetSelection(target =>
        {
            unit.TurnAction(target, activeUnits, actionType);
            isWaitingPlayerAction = false;
        });
    }

    private void StartTargetSelection(Action<Unit> callback)
    {
        IsWaitingTarget = true;
        onTargetSelected = callback;

        SetTargetsOutline();
    }

    private void SetTargetsOutline()
    {
        List<Unit> targets = TargetSystem.GetValidTargets(currentUnit, activeUnits, currentSkill);

        foreach (Unit target in targets)
        {
            Material material = GameManager.Instance.gameResources.targetOutlineMaterial;

            if (material == null) return;

            target.SetMaterial(material);
        }
    }

    #endregion

    #region Cleanup

    private void HandleCombatEnd(CombatEndResult result)
    {
        Clear();
    }

    private void HandleWaveEnd()
    {
        Clear();
    }

    private void Clear()
    {
        foreach (Unit unit in activeUnits)
        {
            unit.OnDeath -= HandleUnitDeath;
            unit.OnUnitSelected -= HandleUnitSelected;
        }

        activeUnits.Clear();
        isWaitingPlayerAction = false;
        IsWaitingTarget = false;
        onTargetSelected = null;
    }

    #endregion
}
