public class CombatStateMachine
{
    public ICombatState CurrentState { get; private set; }

    public void ChangeState(ICombatState state)
    {
        CurrentState = state;
        CurrentState.Enter();
    }    
}

public interface ICombatState
{
    void Enter();
}