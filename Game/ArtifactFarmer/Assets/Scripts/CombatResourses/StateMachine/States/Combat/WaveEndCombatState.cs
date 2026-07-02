public class WaveEndCombatState : ICombatState
{
    public void Enter()
    {
        EndWave();
    }

    private void EndWave()
    {
        CombatContext context = CombatManager.Instance.Context;
        context.ClearWaveData();
        context.AdvanceWaveIndex();

        if (context.CurrentWaveIndex == context.WaveCount)
        {
            CombatManager.Instance.ChangeCombatState(CombatState.Victory);
            return;
        }
        
        CombatManager.Instance.ChangeCombatState(CombatState.WaveStart);
    }
}
