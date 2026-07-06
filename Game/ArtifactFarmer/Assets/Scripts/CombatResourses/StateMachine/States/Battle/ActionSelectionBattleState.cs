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
            context.SetCurrentSkill(unit.GetSkill(ActionType.Enemy));
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
        
        context.SetCurrentSkill(unit.GetSkill(type));

        CombatManager.Instance.CombatUIManager.CloseActionPanel();
        isWaitingForPlayer = false;
    }
}
