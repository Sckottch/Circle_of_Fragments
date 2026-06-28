using UnityEngine;

public class BattleCombatState : ICombatState
{
    private CombatStateMachine battleStateMachine = new ();
    public BattleState CurrentState { get; private set; } = BattleState.Idle;

    //States
    private TurnStartBattleState turnStartState = new ();
    private ActionSelectionBattleState actionSelectionState = new ();
    private TargetSelectionBattleState targetSelectionState = new ();
    private ActionResultBattleState actionResultState = new ();
    private TurnEndBattleState turnEndState = new ();

    public void Enter()
    {
        
    }

    public void ChangeState(BattleState state)
    {
        switch (state)
        {
            case BattleState.TurnStart:
                battleStateMachine.ChangeState(turnStartState);
                break;

            case BattleState.ActionSelection:
                battleStateMachine.ChangeState(actionSelectionState);
                break;

            case BattleState.TargetSelection:
                battleStateMachine.ChangeState(targetSelectionState);
                break;

            case BattleState.ActionResult:
                battleStateMachine.ChangeState(actionResultState);
                break;

            case BattleState.TurnEnd:
                battleStateMachine.ChangeState(turnEndState);
                break;

            default: break;
        }
    }
}
