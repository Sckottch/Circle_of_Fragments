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

        Unit activeUnit = context.ActiveUnit; //por ora sem uso, mas será usado para notificações e sistema de buffs no futuro

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

        context.ClearTurnData();
        battleState.ChangeState(BattleState.TurnStart);
    }
}
