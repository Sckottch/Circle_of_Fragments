public class TurnStartBattleState : ICombatState
{
    private BattleCombatState battleState;

    public TurnStartBattleState(BattleCombatState battleState)
    {
        this.battleState = battleState;
    }

    public void Enter()
    {
        SetNextUnitToAct(); 
    }

    private void SetNextUnitToAct()
    {
        CombatContext context = CombatManager.Instance.Context;
        UnitTurnData data = TurnService.GetNextUnitToAct(context);
        Unit unit = data.Unit;

        context.SetActiveUnit(unit);
        data.RecalculateActionValue();
        CombatManager.Instance.Events.TurnOrderChanged();
        CombatManager.Instance.Events.TurnStarted(unit);
        
        if (!unit.CanAct())
        {
            battleState.ChangeState(BattleState.TurnEnd);
            return;
        }
        
        battleState.ChangeState(BattleState.ActionSelection);
    }
}
