public class TurnEndBattleState : ICombatState
{
    private BattleCombatState battleState;

    public TurnEndBattleState(BattleCombatState battleState)
    {
        this.battleState = battleState;
    }

    public void Enter()
    {
        EndTurn();
    }

    private void EndTurn()
    {
        CombatContext context = CombatManager.Instance.Context;

        Unit activeUnit = context.ActiveUnit;
        CombatManager.Instance.Events.TurnEnd(activeUnit);
        context.ClearTurnData();

        if (!context.HasEnemiesAlive && !context.HasEnemiesToSpawn)
        {
            battleState.BattleEnded();
            return;   
        }

        if (!context.HasPlayersAlive)
        {
            battleState.PlayerDefeated();
            return;
        }

        battleState.ChangeState(BattleState.TurnStart);
    }
}
