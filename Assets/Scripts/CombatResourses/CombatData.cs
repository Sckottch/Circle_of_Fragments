using System.Collections.Generic;

public class CombatData
{
    public List<PlayerUnitData> playerUnits;
    public List<WaveDataSO> waves;

    public CombatData(PlayerCombatData playerData, CombatDataSO combatData)
    {
        playerUnits = playerData.playerUnits;
        waves = combatData.waves;
    }
}
