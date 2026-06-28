using UnityEngine;

public class CombatManager : SingletonMonoBehaviour<CombatManager>
{
    public CombatContext Context { get; private set; }

    private CombatStateMachine combatStateMachine = new ();
    public CombatState CurrentState { get; private set; }

    //States
    private IdleCombatState idleState = new ();
    private WaveStartCombatState waveStartState = new ();
    private BattleCombatState battleState = new ();
    private WaveEndCombatState waveEndState = new ();
    private VictoryCombatState victoryState = new ();
    private DefeatCombatState deafeatState = new ();

    public void ChangeCombatState(CombatState state)
    {
        switch (state)
        {
            case CombatState.Idle:
                combatStateMachine.ChangeState(idleState);
                break;

            case CombatState.WaveStart:
                combatStateMachine.ChangeState(waveStartState);
                break;

            case CombatState.Battle:
                combatStateMachine.ChangeState(battleState);
                break;

            case CombatState.WaveEnd:
                combatStateMachine.ChangeState(waveEndState);
                break;

            case CombatState.Victory:
                combatStateMachine.ChangeState(victoryState);
                break;

            case CombatState.Defeat:
                combatStateMachine.ChangeState(deafeatState);
                break;            

            default: break;
        }
    }
    
    public void ChangeBattleState(BattleState state)
    {
        battleState.ChangeState(state);
    }
}