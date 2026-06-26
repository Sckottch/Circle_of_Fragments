using System.Collections.Generic;

public class PlayerCombatData
{
    public List<PlayerUnitData> playerUnits;

    public PlayerCombatData(List<PlayerUnitData> units)
    {
        playerUnits = units;
    }
}
