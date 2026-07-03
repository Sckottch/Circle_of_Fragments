using System.Collections.Generic;

public class WaveStartCombatState : ICombatState
{
    public void Enter()
    {
        SetupWave();
    }

    private void SetupWave()
    {
        CombatContext context = CombatManager.Instance.Context;
        CombatSetup setup = CombatManager.Instance.CombatSetup;

        WaveDataSO currentWave = context.GetWave(context.CurrentWaveIndex);
        context.SetCurrentWave(currentWave);

        List<EnemyUnit> enemyUnits = new ();

        while (enemyUnits.Count < currentWave.maxEnemies && context.HasEnemiesToSpawn)
        {
            EnemySpawnInfo enemyInfo = context.GetNextEnemySpawn();
            enemyUnits.Add(setup.SpawnEnemyUnit(enemyInfo));
        }

        List<Unit> activeUnits = new ();
        activeUnits.AddRange(context.PlayerUnits);
        activeUnits.AddRange(enemyUnits);

        context.SetActiveUnits(activeUnits);

        CombatManager.Instance.Events.WaveStarted(activeUnits);
        CombatManager.Instance.ChangeCombatState(CombatState.Battle);
    }
}
