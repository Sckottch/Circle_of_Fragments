using System.Collections.Generic;
using UnityEngine;

public class Profile
{
    public string ProfileId { get; private set; } = System.Guid.NewGuid().ToString();

    public string PlayerName { get; private set; }
    public int PlayerLevel { get; private set; }
    public int ExperiencePoints { get; private set; }

    public int CurrentGold { get; private set; }
    public int CurrentFragments { get; private set; }
    public List<PlayerUnitData> PlayerUnits { get; private set; } = new();
    //Inventory will be added here later

    public Profile(string playerName)
    {
        PlayerName = playerName;
        PlayerLevel = 1;
        ExperiencePoints = 0;
        CurrentGold = 1000;
        CurrentFragments = 0;
    }
}
