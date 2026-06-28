using System.Collections.Generic;

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

    //Turns
    public IReadOnlyList<UnitTurnData> TurnData => turnData;
    private List<UnitTurnData> turnData = new ();
    public int CurrentTurn { get; private set; }

    //Constructors
    public CombatContext(List<PlayableUnit> playerUnits, List<WaveDataSO> waves)
    {
        this.playerUnits = new (playerUnits);
        this.waves = new (waves);
    }

    //General
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

    //Enemy Setup
    public bool HasEnemiesToSpawn => enemySpawnQueue.Count > 0;
    public EnemySpawnInfo GetNextEnemySpawn() => enemySpawnQueue.Dequeue();

    //Waves
    public WaveDataSO GetWave(int index) => waves[index];
    public int WaveCount => waves.Count;
    public void SetCurrentWave(WaveDataSO wave) => CurrentWave = wave;

    //Turns
    public void AdvanceTurn() => CurrentTurn++;
}