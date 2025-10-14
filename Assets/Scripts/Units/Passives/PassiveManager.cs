using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : SingletonMonoBehaviour<PassiveManager>
{
    public Dictionary<Unit, PassiveBase> AllPassives { get; private set; } = new Dictionary<Unit, PassiveBase>();
    public List<Unit> ActiveUnits { get; private set; } = new List<Unit>();

    private void OnEnable()
    {
        WaveManager.OnWaveStarted += HandleWaveStarted;
        WaveManager.OnWaveEnded += HandleWaveEnded;
        //CombatSetup.OnEnemyUnitAdded += HandleEnemyUnitAdded;
    }

    private void OnDisable()
    {
        WaveManager.OnWaveStarted -= HandleWaveStarted;
        WaveManager.OnWaveEnded -= HandleWaveEnded;
        //CombatSetup.OnEnemyUnitAdded -= HandleEnemyUnitAdded;
    }

    #region Setup

    private void HandleWaveStarted(List<PlayableUnit> playerUnits, List<EnemyUnit> enemyUnits)
    {
        ActiveUnits.Clear();
        AllPassives.Clear();
        ActiveUnits.AddRange(playerUnits);
        ActiveUnits.AddRange(enemyUnits);

        SetupUnitsPassives();
    }

    private void SetupUnitsPassives()
    {
        foreach (Unit unit in ActiveUnits)
        {
            if (!unit.IsPlayer)
            {
                return; // Only players have passives for now
            }
            else
            {
                PlayableUnitSO unitData = unit.UnitData as PlayableUnitSO;

                PassiveBase passive = Instantiate(unitData.Passive);

                passive.Initialize(unit);
                AllPassives.Add(unit, passive);
            }
        }
    }

    #endregion

    #region Cleanup

    private void HandleWaveEnded()
    {
        Clear();
    }

    private void Clear()
    {
        foreach (var passive in AllPassives.Values)
        {
            passive.CleanUp();
            Destroy(passive);
        }
        AllPassives.Clear();
        ActiveUnits.Clear();
    }

    #endregion

}
