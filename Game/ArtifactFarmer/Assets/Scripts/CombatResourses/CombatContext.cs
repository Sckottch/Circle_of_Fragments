using System.Collections.Generic;
using System.Linq;

public class CombatContext
{
    //General
    public IReadOnlyList<Unit> ActiveUnits => activeUnits;
    private List<Unit> activeUnits = new ();
    public Unit ActiveUnit { get; private set; }
    
    //Setup
    private Queue<EnemySpawnInfo> enemySpawnQueue;
    public IReadOnlyList<PlayableUnit> PlayerUnits => playerUnits;
    private List<PlayableUnit> playerUnits;

    //Waves
    private List<WaveDataSO> waves;
    public WaveDataSO CurrentWave { get; private set; }
    public int CurrentWaveIndex { get; private set; }

    //Turns
    public IReadOnlyList<UnitTurnData> TurnData => turnData;
    private List<UnitTurnData> turnData = new ();
    public float CurrentTurnActionValue { get; private set; }
    public int CurrentTurn { get; private set; }
    private const int turnActionValue = 250;
    
    //Battle
    public Skill CurrentSkill { get; private set; }
    public Unit CurrentTarget { get; private set; }
    public bool IsWaitingForTarget { get; private set; }

    //Constructors
    public CombatContext(List<PlayableUnit> playerUnits, List<WaveDataSO> waves)
    {
        this.playerUnits = new (playerUnits);
        this.waves = new (waves);
    }

    //General
    public void SetActiveUnits(List<Unit> units)
    {
        activeUnits.Clear();

        foreach (Unit unit in units)
        {
            AddActiveUnit(unit);
        }
    }

    public void AddActiveUnit(Unit unit)
    {
        UnitTurnData turnData = new (unit);

        activeUnits.Add(unit);
        this.turnData.Add(turnData);
    }

    public void RemoveActiveUnit(Unit unit)
    {
        activeUnits.Remove(unit);
        turnData.RemoveAll(data => data.Unit == unit);
    }

    public void SetActiveUnit(Unit unit) => ActiveUnit = unit;

    public List<Unit> GetAlivePlayers() => activeUnits.FindAll(u => u.IsPlayer && u.IsAlive());
    public List<Unit> GetAliveEnemies() => activeUnits.FindAll(u => !u.IsPlayer && u.IsAlive());
    public List<Unit> GetAliveUnits() => activeUnits.FindAll(u => u.IsAlive());
    public bool HasPlayersAlive => activeUnits.Any(u => u.IsPlayer && u.IsAlive());
    public bool HasEnemiesAlive => activeUnits.Any(u => !u.IsPlayer && u.IsAlive());

    //Enemy Setup
    public bool HasEnemiesToSpawn => enemySpawnQueue.Count > 0;
    public EnemySpawnInfo GetNextEnemySpawn() => enemySpawnQueue.Dequeue();

    //Waves
    public WaveDataSO GetWave(int index) => waves[index];
    public int WaveCount => waves.Count;
    public void AdvanceWaveIndex() => CurrentWaveIndex++;
    public void SetCurrentWave(WaveDataSO wave)
    {
        CurrentWave = wave;
        enemySpawnQueue = new (wave.enemies);
    }

    public void ClearWaveData()
    {
        turnData.Clear();
        activeUnits.Clear();
        CurrentTurnActionValue = 0;
        CurrentWave = null;
    }
    
    //Turns
    public void AddTurnActionValue(float tickAmount)
    {
        CurrentTurnActionValue += tickAmount;

        while (CurrentTurnActionValue > turnActionValue)
        {
            CurrentTurnActionValue -= turnActionValue;
            CurrentTurn++;
        }
    }
    
    //Battle
    public void SetCurrentSkill(Skill skill) => CurrentSkill = skill;
    public void SetCurrentTarget(Unit unit) => CurrentTarget = unit;
    public void SetIsWaitingForTarget(bool isWaiting) => IsWaitingForTarget = isWaiting;

    public void ClearTurnData()
    {
        ActiveUnit = null;
        CurrentSkill = null;
        CurrentTarget = null;
        IsWaitingForTarget = false;
    }
}