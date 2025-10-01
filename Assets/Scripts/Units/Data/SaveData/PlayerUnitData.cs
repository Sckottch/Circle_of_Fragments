using System;
using UnityEngine;
[System.Serializable]
public class PlayerUnitData
{
    public PlayableUnitSO unitData;
    public int experience;

    public PlayerUnitData(PlayableUnitSO unitData, int experience = 0)
    {
        this.unitData = unitData;
        this.experience = experience;
    }

}
