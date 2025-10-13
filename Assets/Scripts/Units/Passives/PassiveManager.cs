using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : SingletonMonoBehaviour<PassiveManager>
{
    public Dictionary<Unit, PassiveBase> AllPassives { get; private set; } = new Dictionary<Unit, PassiveBase>();
    public List<Unit> ActiveUnits { get; private set; } = new List<Unit>();

    
}
