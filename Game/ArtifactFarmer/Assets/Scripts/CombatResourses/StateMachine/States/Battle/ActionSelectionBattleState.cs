using System.Collections;
using UnityEngine;

public class ActionSelectionBattleState : ICombatState
{
    private BattleCombatState battleState;
    private bool isWaitingForPlayer;

    public ActionSelectionBattleState(BattleCombatState battleState)
    {
        this.battleState = battleState;
    }
    public void Enter()
    {
        CombatManager.Instance.StartRoutine(ActionSelectionRoutine());    
    }

    private IEnumerator ActionSelectionRoutine()
    {
        CombatContext context = CombatManager.Instance.Context;
        Unit unit = context.ActiveUnit;

        if (!unit.IsPlayer)
        {
            EnemyUnit enemy = unit as EnemyUnit;
            context.SetCurrentSkill(enemy.BasicSkill);
            battleState.ChangeState(BattleState.TargetSelection);
            yield break;
        }
        
        CombatUIManager uiManager = CombatManager.Instance.CombatUIManager;
        isWaitingForPlayer = true;
        
        uiManager.OpenActionPanel(unit, SkillSelectionSignal);
        yield return new WaitUntil(() => !isWaitingForPlayer);
        
        battleState.ChangeState(BattleState.TargetSelection);
    }

    private void SkillSelectionSignal(Unit unit, ActionType type)
    {
        CombatContext context = CombatManager.Instance.Context;
        PlayableUnit playerUnit = unit as PlayableUnit;

        if (playerUnit == null)
        {
            isWaitingForPlayer = false;
            return;
        }
        
        switch (type)
        {
            case ActionType.BasicAttack:
                context.SetCurrentSkill(playerUnit.BasicSkill);
                break;
            
            case ActionType.Skill:
                context.SetCurrentSkill(playerUnit.SpecialSkill);
                break;
                
            case ActionType.Ultimate:
                context.SetCurrentSkill(playerUnit.UltimateSkill);
                break;
        }

        CombatManager.Instance.CombatUIManager.CloseActionPanel();
        isWaitingForPlayer = false;
    }
}
