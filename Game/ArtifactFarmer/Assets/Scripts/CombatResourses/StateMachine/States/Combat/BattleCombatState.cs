using UnityEngine;

public class BattleCombatState : ICombatState
{
    private CombatStateMachine battleStateMachine = new ();
    public BattleState CurrentState { get; private set; } = BattleState.Idle;

    //States
    private TurnStartBattleState turnStartState;
    private ActionSelectionBattleState actionSelectionState;
    private TargetSelectionBattleState targetSelectionState;
    private ActionResultBattleState actionResultState;
    private TurnEndBattleState turnEndState;

    public BattleCombatState()
    {
        turnStartState = new (this);
        actionSelectionState = new (this);
        targetSelectionState = new (this);
        actionResultState = new (this);
        turnEndState = new (this);
    }

    public void Enter()
    {
       ChangeState(BattleState.TurnStart); 
    }

    public void ChangeState(BattleState state)
    {
        CurrentState = state;

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

    public void BattleEnded ()
    {
        CombatManager.Instance.ChangeCombatState(CombatState.WaveEnd);
        CurrentState = BattleState.Idle;
    }

    public void PlayerDefeated()
    {
        CombatManager.Instance.ChangeCombatState(CombatState.Defeat);
        CurrentState = BattleState.Idle;
    }
}
